
using SIV.Api.Authorization.Models;
using SqlSugar;

namespace SIV.Api.Authorization
{
    public interface IAuthDataService
    {
        public Dictionary<int, UserRoleHierarchy> GetHierarchy();
    }

    public class AuthDataService : BackgroundService, IAuthDataService
    {
        private readonly SqlSugarClient sqlSugarClient;
        public List<User> Users { get; set; } = new List<User>();
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<ApiModel> ApiModels { get; set; } = new List<ApiModel>();


        public AuthDataService(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var sc = scope.ServiceProvider.GetService<SqlSugarClient>();

                sqlSugarClient = sc;
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("AuthDataService StartAsync");
            var uList = await sqlSugarClient.Queryable<User>().Where(x => !x.IsDeleted).ToListAsync();
            var rList = await sqlSugarClient.Queryable<Role>().Where(x => !x.IsDeleted).ToListAsync();
            var apiList = await sqlSugarClient.Queryable<ApiModel>().Where(x => !x.IsDeleted).ToListAsync();

            Users.Clear();
            Roles.Clear();
            ApiModels.Clear();

            Users.AddRange(uList);
            Roles.AddRange(rList);
            ApiModels.AddRange(apiList);
        }



        /// <summary>
        /// 获取用户权限关系
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, UserRoleHierarchy> GetHierarchy()
        {
            Dictionary<int, UserRoleHierarchy> keyValuePairs = new Dictionary<int, UserRoleHierarchy>();

            foreach (var user in Users)
            {
                UserRoleHierarchy userRoleHierarchy = new()
                {
                    Id = user.Id
                };

                var role = Roles.FirstOrDefault(x => x.Id == user.RoleId);

                if (role == null)
                {
                    keyValuePairs.Add(user.Id, userRoleHierarchy);
                    continue;
                }

                userRoleHierarchy.RoleId = role.Id;
                userRoleHierarchy.RoleName = role.Name;

                if (role.ApiIds == "ALL")
                {
                    userRoleHierarchy.ApiIds.AddRange(ApiModels.Select(x => x.Id));
                    userRoleHierarchy.ApiPaths.AddRange(ApiModels.Select(x => x.Path));

                    keyValuePairs.Add(user.Id, userRoleHierarchy);
                }
                else
                {
                    var urids = role.ApiIds.Split(",").ToList().ConvertAll(x => int.Parse(x));

                    var apis = ApiModels.Where(x => urids.Contains(x.Id));

                    userRoleHierarchy.ApiIds.AddRange(apis.Select(x => x.Id));
                    userRoleHierarchy.ApiPaths.AddRange(apis.Select(x => x.Path));

                    keyValuePairs.Add(user.Id, userRoleHierarchy);
                }
            }

            return keyValuePairs;
        }

    }

    public class UserRoleHierarchy
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<int> ApiIds { get; set; } = new List<int>();
        public List<string> ApiPaths { get; set; } = new List<string>();

    }
}

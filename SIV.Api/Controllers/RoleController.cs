using Aspose.Cells.Charts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SIV.Api.Authorization.Models;
using SIV.Api.DTO;
using SqlSugar;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public RoleController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Role, Role>()
             .ForMember(x => x.CreateTime, op => op.Ignore())
             .ForMember(x => x.UpdateTime, op => op.Ignore())
             .ForMember(x => x.IsDeleted, op => op.Ignore())
             .ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null))
             );
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public AjaxResult<List<Role>> Get()
        {
            var result = new AjaxResult<List<Role>>();
            result.Code = 200;

            result.Data = sqlSugarClient.Queryable<Role>().ToList();

            return result;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Add(Role role)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var count = sqlSugarClient.Insertable(role).ExecuteCommand();

            if (count > 0)
                result.Message = "添加成功";
            else
                result.Message = "添加失败";
            return result;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(Role role)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbRole = sqlSugarClient.Queryable<Role>().Where(x => x.Id == role.Id && !x.IsDeleted).First();

            mapper.Map(role, dbRole);

            var count = sqlSugarClient.Updateable(role).ExecuteCommand();
            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
            }
            else
            {
                result.Success = false;
                result.Message = "更新失败";
            }

            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteRole role)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbRole = sqlSugarClient.Queryable<Role>().Where(x => x.Id == role.Id && !x.IsDeleted).First();

            dbRole.IsDeleted = true;
            dbRole.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbRole).ExecuteCommand();

            if (count > 0)
            { 
                var userList = sqlSugarClient.Queryable<User>().Where(x => !x.IsDeleted && x.RoleId == role.Id).ToList();

                foreach (var user in userList)
                {
                    user.RoleId = 0;
                    user.UpdateTime = DateTime.Now;
                }

                sqlSugarClient.Updateable(userList).ExecuteCommand();

                result.Success = true;
                result.Message = "删除成功";
            }
            else
            {
                result.Success = false;
                result.Message = "删除失败";
            }

            return result;
        }
    }

    public class DeleteRole
    {
        public int Id { get; set; }
    }
}

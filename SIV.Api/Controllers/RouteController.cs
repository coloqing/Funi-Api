using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIV.Api.DTO;
using SIV.Api.Models;
using SqlSugar;
using System.Dynamic;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public RouteController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RouteDTO, Models.Route>().ForAllMembers(opt => opt.Condition((src, dest, svalue) => { 
                    return svalue != null; }));
                cfg.CreateMap<Models.Route, RouteDTO>(); 
            }
);
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public AjaxResult<List<RouteDTO>> Get()
        {
            var result = new AjaxResult<List<RouteDTO>>();
            result.Code = 200;

            var data = sqlSugarClient.Queryable<Models.Route>().Where(x => !x.IsDeleted).ToList();

            result.Data = mapper.Map<List<RouteDTO>>(data);

            return result;
        }

        /// <summary>
        /// 获取结构化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Structure")]
        public AjaxResult<object> GetStructure()
        {
            var result = new AjaxResult<object>();
            result.Code = 200;
            var query = sqlSugarClient.Queryable<Models.Route>().Where(x => !x.IsDeleted);
            var data = query.ToList();

            var routeRoots = data.Where(x => x.ParentId == 0);

            var ob = new List<ExpandoObject>();

            foreach (var item in routeRoots)
            {
                dynamic root = new ExpandoObject();
                root.Id = item.Id;
                root.ParentId = item.ParentId;
                root.Path = item.Path;
                root.Name = item.Name;
                root.Component = item.Component;
                root.Redirect = item.Redirect;
                root.AlwaysShow = item.AlwaysShow;
                root.Meta = new
                {
                    Title = item.Title,
                    Icon = item.Icon
                };

                FindChildrens(ref root, data);

                ob.Add(root);
            }

            result.Data = ob;

            return result;
        }

        [NonAction]
        public void FindChildrens(ref dynamic root, List<Models.Route> list)
        {
            var id = root.Id;
            foreach (var item in list.Where(x => x.ParentId == id))
            {
                dynamic child = new ExpandoObject();
                child.Id = item.Id;
                child.ParentId = item.ParentId;
                child.Path = item.Path;
                child.Name = item.Name;
                child.Component = item.Component;
                child.Redirect = item.Redirect;
                child.AlwaysShow = item.AlwaysShow;
                child.Meta = new
                {
                    Title = item.Title,
                    Icon = item.Icon,
                    Roles = item.Roles.Split(',')
                };

                FindChildrens(ref child, list);

                if (!HasProperty(root, "Children"))
                    root.Children = new List<dynamic>();

                root.Children.Add(child);
            }
        }

        [NonAction]
        static bool HasProperty(dynamic obj, string propertyName)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(propertyName);
            return obj.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] RouteDTO route)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbroute = mapper.Map<Models.Route>(route);

            dbroute.CreateTime = DateTime.Now;
            dbroute.UpdateTime = DateTime.Now;
            dbroute.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbroute).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "新增成功";
            }
            else
            {
                result.Success = false;
                result.Message = "新增失败";
            }

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(RouteDTO route)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbroute = sqlSugarClient.Queryable<Models.Route>().First(x => x.Id == route.Id && !x.IsDeleted);

            mapper.Map(route, dbroute);

            dbroute.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbroute).ExecuteCommand();

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
        /// 删除
        /// </summary>
        /// <param name="dRoute"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteRoute dRoute)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbroute = sqlSugarClient.Queryable<Models.Route>().First(x => x.Id == dRoute.Id && !x.IsDeleted);

            dbroute.UpdateTime = DateTime.Now;
            dbroute.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbroute).ExecuteCommand();

            if (count > 0)
            {
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

    public class DeleteRoute
    {
        public int Id { get; set; }
    }
}

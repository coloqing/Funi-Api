using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public RoleController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
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

            var count = sqlSugarClient.Updateable(role).ExecuteCommand();
            if (count > 0)
                result.Message = "更新成功";
            else
                result.Message = "更新失败";

            return result;
        }
    }
}

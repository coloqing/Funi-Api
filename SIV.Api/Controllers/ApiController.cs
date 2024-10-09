using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.Authorization.Models;
using SqlSugar;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;

        public ApiController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 获取所有api信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult<List<ApiModel>> Get()
        {
            var result = new AjaxResult<List<ApiModel>>();
            result.Code = 200;
            result.Success = true;
            result.Data =  sqlSugarClient.Queryable<ApiModel>().ToList();

            return result;
        }

        /// <summary>
        /// 添加api信息
        /// </summary>
        /// <param name="apiModel"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<List<ApiModel>> Add(ApiModel apiModel)
        {
            var result = new AjaxResult<List<ApiModel>>();
            result.Code = 200;

            var count = sqlSugarClient.Insertable(apiModel).ExecuteCommand();

            return result;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SIV.Api.Authorization.Models;
using SqlSugar;
using Util.DTO;
using SIV.Util;
using SIV.Entity;

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
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">行数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<ApiModel>> Get(int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<ApiModel>().Where(x => !x.IsDeleted);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

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

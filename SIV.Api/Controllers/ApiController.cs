using Microsoft.AspNetCore.Mvc;
using SIV.Api.Authorization.Models;
using SqlSugar;
using Util.DTO;
using SIV.Util;
using SIV.Entity;
using AutoMapper;
using Aspose.Cells.Charts;
using SIV.Api.DTO;
using SIV.Api.Models;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public ApiController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApiModelDTO, ApiModel>().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                cfg.CreateMap<ApiModel, ApiModelDTO>();
            }
            );
            mapper = config.CreateMapper();
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
        public AjaxResult<string> Add(ApiModelDTO apiModel)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbapi = mapper.Map<ApiModel>(apiModel);

            dbapi.CreateTime = DateTime.Now;
            dbapi.UpdateTime = DateTime.Now;
            dbapi.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbapi).ExecuteCommand();

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
        /// <param name="apiModel"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(ApiModelDTO apiModel)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbapi = sqlSugarClient.Queryable<ApiModel>().First(x => x.Id == apiModel.Id && !x.IsDeleted);

            mapper.Map(apiModel, dbapi);

            dbapi.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbapi).ExecuteCommand();

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
        /// <param name="deleteEntityId"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteEntityId deleteEntityId)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbindicators = sqlSugarClient.Queryable<ApiModel>().First(x => x.Id == deleteEntityId.Id && !x.IsDeleted);

            dbindicators.UpdateTime = DateTime.Now;
            dbindicators.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbindicators).ExecuteCommand();

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
}

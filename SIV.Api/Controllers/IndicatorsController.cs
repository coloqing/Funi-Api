using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SIV.Api.Models;
using SqlSugar;
using SIV.Util;
using Util.DTO;

namespace SIV.Api.Controllers
{
    /// <summary>
    /// 性能指标
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorsController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public IndicatorsController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IndicatorsDTO, Indicators>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
            );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 获取性能指标数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<IndicatorsDTO>> Get(int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<Indicators>().Where(x => !x.IsDeleted);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<IndicatorsDTO>
            {
                Data = mapper.Map<List<IndicatorsDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="indicators"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] IndicatorsDTO indicators)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbindicators = mapper.Map<Indicators>(indicators);

            dbindicators.CreateTime = DateTime.Now;
            dbindicators.UpdateTime = DateTime.Now;
            dbindicators.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbindicators).ExecuteCommand();

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
        /// <param name="indicators"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(IndicatorsDTO indicators)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbindicators = sqlSugarClient.Queryable<Indicators>().First(x => x.Id == indicators.Id && !x.IsDeleted);

            mapper.Map(indicators, dbindicators);

            dbindicators.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbindicators).ExecuteCommand();

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

            var dbindicators = sqlSugarClient.Queryable<Device>().First(x => x.Id == deleteEntityId.Id && !x.IsDeleted);

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

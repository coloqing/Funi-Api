using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SqlSugar;
using SIV.Util;
using Util.DTO;
using SIV.Api.Models;
using System.Runtime.CompilerServices;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public ConfigController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient; 
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ConfigDTO, Config>().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                    cfg.CreateMap<Config, ConfigDTO>();
                }
            );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineName">线路</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<ConfigDTO>> Get(string? lineName = default ,int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<Config>().Where(x => !x.IsDeleted);
            query.WhereIF(!string.IsNullOrEmpty(lineName), x => x.LineName == lineName);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<ConfigDTO>
            {
                Data = mapper.Map<List<ConfigDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] ConfigDTO config)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbconfig = mapper.Map<Config>(config);

            dbconfig.CreateTime = DateTime.Now;
            dbconfig.UpdateTime = DateTime.Now;
            dbconfig.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbconfig).ExecuteCommand();

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
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(ConfigDTO config)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbconfig = sqlSugarClient.Queryable<Config>().First(x => x.Id == config.Id && !x.IsDeleted);

            mapper.Map(config, dbconfig);

            dbconfig.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbconfig).ExecuteCommand();

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
        /// <param name="dConfig"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteConfig dConfig)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcocach = sqlSugarClient.Queryable<Config>().First(x => x.Id == dConfig.Id && !x.IsDeleted);

            dbcocach.UpdateTime = DateTime.Now;
            dbcocach.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbcocach).ExecuteCommand();

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

    public class DeleteConfig
    {
        public int Id { get; set; }
    }
}

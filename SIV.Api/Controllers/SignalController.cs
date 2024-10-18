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
    [Route("api/[controller]")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public SignalController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SignalDTO, Signal>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
            );
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<PageResult<SignalDTO>> Get(int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<Signal>().Where(x => !x.IsDeleted);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<SignalDTO>
            {
                Data = mapper.Map<List<SignalDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] SignalDTO signal)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbsignal = mapper.Map<Signal>(signal);

            dbsignal.CreateTime = DateTime.Now;
            dbsignal.UpdateTime = DateTime.Now;
            dbsignal.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbsignal).ExecuteCommand();

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
        /// <param name="signal"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(SignalDTO signal)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbsignal = sqlSugarClient.Queryable<Signal>().First(x => x.Id == signal.Id && !x.IsDeleted);

            mapper.Map(signal, dbsignal);

            dbsignal.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbsignal).ExecuteCommand();

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

            var dbsignal = sqlSugarClient.Queryable<Device>().First(x => x.Id == deleteEntityId.Id && !x.IsDeleted);

            dbsignal.UpdateTime = DateTime.Now;
            dbsignal.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbsignal).ExecuteCommand();

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

    public class DeleteEntityId
    {
        public int Id { get; set; }
    }

}

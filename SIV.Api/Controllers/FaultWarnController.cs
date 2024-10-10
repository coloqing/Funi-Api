using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SIV.Entity.Tables;
using SIV.Entity;
using Util.DTO;
using AutoMapper;
using SIV.Controllers;
using SqlSugar;
using SIV.Util;
using System.DirectoryServices.Protocols;

namespace SIV.Api.Controllers
{   
    /// <summary>
    /// 故障预警
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FaultWarnController : ControllerBase
    {
        private readonly SqlSugarClient _db;
        private readonly AppSettings _appSettings;
        private readonly ILogger<TrainController> _logger;
        private readonly IMapper _mapper;
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        public FaultWarnController(
            ILogger<TrainController> logger,
            SqlSugarClient db,
            IMapper mapper,
            AppSettings appSettings)
        {
            _logger = logger;
            _db = db;
            _appSettings = appSettings;
            _mapper = mapper;
          
        }

        /// <summary>
        /// 获取故障预警列表
        /// </summary>
        /// <param name="name">预警/报警名称</param>
        /// <param name="type">类型</param>
        /// <param name="state">状态</param>
        /// <param name="lineId">线路Id</param>
        /// <param name="trainId">列车Id</param>
        /// <param name="carriageId">车厢Id</param>
        /// <param name="itemSys">子系统</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="sortFile">排序字段</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">每页行数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<FaultOrWarn>> GetData(string? name,int? type, int? state, long? lineId, long? trainId,long? carriageId, string? itemSys,DateTime? startTime,DateTime? endTime, string sortFile = "Percent", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            var where = Expressionable.Create<FaultOrWarn>();
            if (name != null)
                where.And(c => c.Name.Contains(name));
            if (type != null)
                where.And(c => c.Type == type);
            if (state != null)
                where.And(c => c.State == state);
            if (lineId != null)
                where.And(c => c.LineId == lineId);
            if (trainId != null)
                where.And(c => c.TrainId == trainId);
            if (carriageId != null)
                where.And(c => c.CarriageId == carriageId);
            if (itemSys != null)
                where.And(c => c.SubSystem == itemSys);
            if (startTime != null)
                where.And(c => c.CreateTime >= startTime);
            if (endTime != null)
                where.And(c => c.EndTime <= endTime);
            var exp = where.ToExpression();

            var fault = _db.Queryable<FaultOrWarn>().Where(exp);

            var result = await fault.GetPageResultAsync(sortFile, sortType, pageIndex, pageRow);

            return result;
        }

        /// <summary>
        /// 获取故障详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("FaultInfo")]
        public async Task<AjaxResult<FaultOrWarn>> GetTheData(long? id)
        {
            if (id != null)
            {
                throw new Exception("id不可以为空");
            }
            var result = new AjaxResult<FaultOrWarn>();
            var q = await _db.Queryable<FaultOrWarn>().FirstAsync(x => x.Id == id);
            result.Data = q;
            return result;
        }
    }
}

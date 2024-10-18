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
        /// <param name="lineId">线路号</param>
        /// <param name="trainNum">列车号</param>
        /// <param name="state">状态 0=未消除 1=车载消除</param>
        /// <param name="alarmName">预警名称</param>
        /// <param name="alarmType">预警类型 2-预警 3-报警</param>
        /// <param name="sortFile">排序字段</param>
        /// <param name="sortType">排序方式</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">行数</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<FaultOrWarn>> GetData(int? lineId = default, string? trainNum = default, int? state = default, string? alarmName = default, int? alarmType = default, string? sortFile = "CreateTime", string? sortType = "desc", int pageIndex = 1, int pageRow = 7,DateTime? startTime = default,DateTime? endTime = default)
        {
            var where = Expressionable.Create<FaultOrWarn>().And(x => !x.IsDeleted);

            where.AndIF(lineId != default, x => x.LineId == lineId);
            where.AndIF(trainNum != default, x => x.TrainNumber == trainNum);
            where.AndIF(state != default, x => x.State == state);
            where.AndIF(alarmName != default, x => x.Name == alarmName);
            where.AndIF(alarmType != default, x => x.Grade == alarmType); 
            where.AndIF(startTime != default, x => x.CreateTime >= startTime);
            where.AndIF(endTime != default, x => x.EndTime <= endTime);

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

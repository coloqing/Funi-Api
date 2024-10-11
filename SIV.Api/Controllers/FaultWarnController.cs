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
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<FaultOrWarn>> GetData(string? sortFile = "CreateTime", string? sortType = "desc", int pageIndex = 1, int pageRow = 7)
        {
            var where = Expressionable.Create<FaultOrWarn>().And(x => !x.IsDeleted);

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

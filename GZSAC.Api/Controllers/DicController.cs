using GZSAC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAC.Entity;
using SAC.Util.DTO;
using SqlSugar;
using System.Security.Principal;
using Util.DTO;

namespace GZSAC.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class DicController : ControllerBase
    {
        private readonly SqlSugarClient _db;

        private readonly AppSettings _appSettings;

        private readonly ILogger<DicController> _logger;

        public DicController(
            ILogger<DicController> logger,
            SqlSugarClient db,
            AppSettings appSettings)
        {
            _logger = logger;
            _db = db;
            _appSettings = appSettings;
        }

        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDicData")]
        public async Task<AjaxResult<List<BaseDicEntity>>> GetDicData(string parentCode)
        {

            string sql = $@"SELECT d2.*  
                            FROM TB_SYS_DIC d1  
                            INNER JOIN TB_SYS_DIC d2 ON d1.Id = d2.ParentId  
                            WHERE d1.Code = '{parentCode}';";
            var dic = await _db.SqlQueryable<TB_SYS_DIC>(sql).ToListAsync();
           
            return new AjaxResult<List<BaseDicEntity>>() { Data = dic.Select(a => new BaseDicEntity() { Key = a.Code, Value = a.Name }).ToList() };

        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Reflection;

namespace SIV.Api.Controllers
{
    /// <summary>
    /// 系统控制
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly SqlSugarClient _db;
        private readonly AppSettings _appSettings;
        private readonly ILogger<TrainController> _logger;

        public SystemController(
            ILogger<TrainController> logger,
            SqlSugarClient db,
            IMapper mapper,
            AppSettings appSettings)
        {
            _logger = logger;
            _db = db;
            _appSettings = appSettings;       
        }



        /// <summary>
        /// 创建新表加日期
        /// </summary>
        [HttpGet]
        public async Task InitTables()

        {
            try
            {
                _logger.LogInformation("开始更新表结构...");

                // 加载程序集并获取相关类型  
                var assembly = Assembly.LoadFrom("SIV.Entity.dll").GetTypes().Where(x => x.Namespace == "SIV.Entity").ToList();

                foreach (Type item in assembly)
                {
                    _db.CodeFirst.InitTables(item);
                }

                _logger.LogInformation("表结构更新完成");
            }
            catch (Exception ex)
            {
                _logger.LogError("表结构更新失败: " + ex.ToString());
            }
        }

       
    }
}

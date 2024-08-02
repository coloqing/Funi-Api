using GZSAC.Api.DTO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAC.Entity;
using SAC.Util;
using SAC.Util.DTO;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace GZSAC.Api
{
    public class FaultWarnService : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<FaultWarnService> _logger;
        private readonly ISqlSugarClient _db;
      

        public FaultWarnService(ILogger<FaultWarnService> logger, ISqlSugarClient dbContext, AppSettings appSettings)
        {
            _logger = logger;
            _db = dbContext;
            _appSettings = appSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("开始同步故障");
                await Console.Out.WriteLineAsync("开始同步故障");
                await AddFaultData();
                
                // 等待一段时间再执行下一次任务  
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        /// <summary>
        /// 添加故障信息
        /// </summary>
        /// <returns></returns>
        private async Task AddFaultData()
        {
            var addFaults = new List<FaultOrWarn>();
            var updateFaults = new List<FaultOrWarn>();

            try
            {
                //获取配置的json数据
            
                var config = _db.Queryable<SYS_Config>().ToList();
                //获取预警时间
                var sj = config.Where(x => x.concode == "FaultSj").First()?.conval;

                //获取线路名称
                var XL = config.Where(x => x.concode == "GZML7S").First()?.conval;
                //获取寿命部件

                string sql = @"SELECT
									pd.jz1zhl1dlqgz, 
									pd.jz1zhl2dlqgz, 
									pd.jz1ysj1dlqgz, 
									pd.jz1ysj2dlqgz, 
									pd.jz1tfjjcqgz, 
									pd.jz1lnfjjcqgz, 
									pd.jz1tfjjjtfjcqgz, 
									pd.jz1ysj1jcqgz, 
									pd.jz1ysj2jcqgz, 
									pd.jz1ysj1gygz, 
									pd.jz1ysj1dygz, 
									pd.jz1ysj2gygz, 
									pd.jz1ysj2dygz, 
									pd.zygz, 
									pd.jjtfnbqgz, 
									pd.jz1zwxdgz, 
									pd.jz1tfj1gzgz, 
									pd.jz1tfj2gzgz, 
									pd.jz1lnfj1gzgz, 
									pd.jz1lnfj2gzgz, 
									pd.jz1bpq1gz, 
									pd.jz1bpq2gz, 
									pd.jz1bpq1txgz, 
									pd.jz1bpq2txgz, 
									pd.jz1xff1gz, 
									pd.jz1xff2gz, 
									pd.jz1hff1gz, 
									pd.jz1hff2gz, 
									pd.kswdcgq1gz, 
									pd.jz1hfcgqgz, 
									pd.jz1xfcgqgz, 
									pd.jz1sfcgq1gz, 
									pd.jz1sfcgq2gz, 
									pd.jz1kqjhqgz, 
									pd.jz1ysj1pqwdcgqgz, 
									pd.jz1ysj1xqwdcgqgz, 
									pd.jz1ysj2pqwdcgqgz, 
									pd.jz1ysj2xqwdcgqgz, 
									pd.jz1ysj1pqwdgz, 
									pd.jz1ysj2pqwdgz, 
									pd.jz1cjmk1txgz, 
									pd.jz1cjmk2txgz, 
									pd.jz1kqzljcmkgz, 
									pd.jz1gyylcgq1gz, 
									pd.jz1dyylcgq1gz, 
									pd.jz1gyylcgq2gz, 
									pd.jz1dyylcgq2gz, 
									pd.jz1qwgz, 
									pd.jz1zdgz, 
									pd.jz1yzgz, 
									pd.jz1ysj1gzgz, 
									pd.jz1ysj2gzgz, 
									pd.jz1zwxd1gz, 
									pd.jz1zwxd2gz, 
									pd.jz1tfj1gsjcqgz, 
									pd.jz1tfj2gsjcqgz, 
									pd.jz1tfj1dsjcqgz, 
									pd.jz1tfj2dsjcqgz, 
									pd.fpfjgz, 
									pd.fpfjjcqgz, 
									pd.fpfjjjtfjcqgz, 
									pd.fpffgz, 
									pd.fhfgz, 
									pd.lch, 
									pd.cxh,
                                    pd.cxhName,
									pd.device_code, 
									pd.yxtzjid
								FROM
									dbo.TB_PARSING_DATAS" + $"_{DateTime.Now.ToString("yyyyMMdd")} " + "AS pd "+
								$@"WHERE
									pd.create_time >= DATEADD(MINUTE,-{sj},GETDATE())";

                var faults = await _db.SqlQueryable<FaultDTO>(sql).ToListAsync();

                var equipments = await _db.Queryable<EquipmentFault>().ToListAsync();

                var propertyCache = new ConcurrentDictionary<string, Func<object, object>>();

                foreach (var data in faults)
                {
                    Type type = data.GetType();

                    equipments= equipments.Where(x => x.CXH == data.cxhName &&x.HvacType == data.yxtzjid).ToList();
                    foreach (var item in equipments)
                    {
                        string propertyName = item.AnotherName;

                        if (propertyName == null)
                        {
                            continue;
                        }

                        // 尝试从缓存中获取属性访问器  
                        if (!propertyCache.TryGetValue(propertyName, out var getValue))
                        {
                            // 如果缓存中不存在，则使用反射创建并添加到缓存中  
                            PropertyInfo propertyInfo = type.GetProperty(propertyName);
                            if (propertyInfo != null)
                            {
                                // 注意：这里我们假设属性可以安全地转换为 object，然后在需要时转换为具体类型  
                                getValue = obj => propertyInfo.GetValue(obj, null);
                                propertyCache.TryAdd(propertyName, getValue); // 尝试添加到缓存中，防止并发时的重复添加  
                            }
                            else
                            {
                                // 如果属性不存在，则可以根据需要处理（例如记录错误、跳过等）  
                                continue; // 这里我们选择跳过当前 item  
                            }
                        }
                        string isFault= getValue(data).ToString();

                        var isAny = _db.Queryable<FaultOrWarn>()
                              .Where(x => x.DeviceCode == data.device_code && x.Code == item.FaultCode)
                              .First();

                        if (isAny == null)
                        {
                            if (isFault == "1")
                            {
                                //创建 faultOrWarn 对象并设置属性
                                var faultOrWarn = new FaultOrWarn
                                {
                                    xlh = XL,
                                    lch = data.lch,
                                    cxh = data.cxh,
                                    DeviceCode = data.device_code,
                                    Code = item.FaultCode,
                                    Type = "1",
                                    State = "0",
                                    createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };
                                addFaults.Add(faultOrWarn);
                            }
                        }
                        else
                        {
                            if (isFault == "0" && isAny.State == "0")
                            {
                                isAny.State = "2";
                                isAny.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                updateFaults.Add(isAny);
                            }
                        }
                    }
                }
                var num = _db.Insertable(addFaults).ExecuteCommand();
                var num1 = _db.Updateable(updateFaults).ExecuteCommand();
                await Console.Out.WriteLineAsync("故障信息同步成功");

            }
            catch (Exception ex)
            {
                _logger.LogError($"故障信息添加失败，{ex.ToString()}");               
            };        
        }

        /// <summary>
        /// 故障推送
        /// </summary>
        /// <param name="newFault"></param>
        /// <param name="endFault"></param>
        /// <returns></returns>
        public async Task FaultSetHttpPost(List<FaultOrWarn> newFault, List<FaultOrWarn> endFault)
        {
            string appId = _appSettings.AppId;
            string appKey = _appSettings.AppKey;
            string baseUrl = _appSettings.BaseUrl;
            string urlType = "";
            string lineCode = _appSettings.LineCode;

            // 构建app_token
            string appToken = $"app_id={appId}&app_key={appKey}&date=" + DateTime.Now.ToString("yyyy-MM-dd");
            string tokenMd5 = Helper.GetMD5String(appToken).ToUpper();
            string url = $"{baseUrl}{urlType}";

            var new_faults = new List<New_FaultsModels>();
            var end_faults = new List<End_FaultsModels>();

            foreach (var item in newFault)
            {
                new_faults.Add(new New_FaultsModels()
                {
                    line_code = "GZML11",
                    train_code = item.lch,
                    coach_no = item.cxh.Substring(2),
                    fault_code = item.Code,
                    access_time = item.createtime.ToString()
                });
            }

            foreach (var item in endFault)
            {
                end_faults.Add(new End_FaultsModels()
                {
                    line_code = "GZML11",
                    train_code = item.lch,
                    fault_code = item.Code,
                    coach_no=item.cxh.Substring(2),
                    access_time=item.updatetime.ToString()
                });
            }

            var request = new Fault_AssessModels() 
            { 
                app_id = appId,
                app_token = appToken,
                new_faults = new_faults,
                end_faults = end_faults
            };

            var trainSta = HttpClientExample.HttpClientPost(url,request);


        }
    }

}

                

        
        
        
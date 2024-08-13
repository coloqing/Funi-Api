using Microsoft.AspNetCore.Mvc;
using SAC.Entity;
using SqlSugar;
using subway_airConditioner;
using SAC.Util;
using Util.DTO;
using Microsoft.Extensions.Options;
using SAC.Util.DTO;
using subway_airConditioner.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using GZSAC.Api.DTO;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using System.Net;
using System.IO;
using GZSAC.Api.DTO.OldDTO;
using System.Collections;

namespace GZSAC.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TrainController : ControllerBase
    {
        private readonly SqlSugarClient _db;

        private readonly AppSettings _appSettings;

        private readonly ILogger<TrainController> _logger;

        public TrainController(
            ILogger<TrainController> logger,
            SqlSugarClient db,
            AppSettings appSettings)
        {
            _logger = logger;
            _db = db;
            _appSettings = appSettings;
        }

        /// <summary>
        /// ��ȡ����ϵͳ�г�ͳ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHttpTrain")]
        public async Task<AjaxResult<List<LCH>>> GetHttpTrain()
        {
            var result = new AjaxResult<List<LCH>>();
            string sql = @"SELECT
	                            a.lch
                            FROM
	                            dbo.LCH AS a";
            var data = _db.SqlQueryable<LCH>(sql).ToList();
            foreach (var item in data)
            {
                item.cxh = item.lch.Substring(0, 2) + "A" + item.lch.Substring(2, 3);
            }
            //result.Data = data;
            //return result;

            string appId = _appSettings.AppId;
            string appKey = _appSettings.AppKey;
            string baseUrl = _appSettings.BaseUrl;
            string urlType = "line-statistics";
            string lineCode = _appSettings.LineCode;

            // ����app_token
            string appToken = $"app_id={appId}&app_key={appKey}&date=" + DateTime.Now.ToString("yyyy-MM-dd");
            string tokenMd5 = Helper.GetMD5String(appToken).ToUpper();
            string url = $"{baseUrl}{urlType}?app_id={appId}&app_token={tokenMd5}&line_code={lineCode}";
            var trainSta = await HttpClientExample.SendGetRequestAsync<HttpTrainStaDTO>(url);

            if (trainSta != null)
            {
                var onlien = trainSta.result_data.FirstOrDefault().online_trains.ToList();
                var depot = trainSta.result_data.FirstOrDefault().depot_trains.ToList();

                foreach (var item in data)
                {
                    if (onlien.Contains(item.lch))
                    {
                        item.State = 2;
                    }
                   
                    if (depot.Contains(item.lch))
                    {
                        item.State = 1;
                    }                
                }
                result.Data = data.OrderBy(x =>x.lch).ToList();
                return result;
            }
            else
            {
                result.Data = data;
                return result;
            }

        }

        /// <summary>
        /// ��ȡ�г�״̬����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTrainSta")]
        public async Task<AjaxResult<List<TrainStaDTO>>> GetTrainSta(string? lch)
        {
            var resultData = new AjaxResult<List<TrainStaDTO>>();
            string sql = @"SELECT
	                            a.lch, 
	                            a.cxh, 
	                            a.device_code, 
	                            a.yxtzjid,                               
                                f.State AS FaultState,
	                            COALESCE(f.Type, a.State) AS State 
                            FROM
	                            dbo.TB_PARSING_NEWDATAS AS a
	                            LEFT JOIN
	                            dbo.FaultOrWarn AS f
	                            ON a.device_code = f.DeviceCode
                            WHERE 
                                f.State = '1'
                                    ";

            var data = await _db.SqlQueryable<TrainStaDTO>(sql).ToListAsync();

            var result = new List<TrainStaDTO>();

            if (!string.IsNullOrEmpty(lch))
            {
                data = data.Where(a => a.lch == lch).ToList();

                result = data.GroupBy(x => x.device_code).Select(group => new TrainStaDTO
                {
                    cxh = group.First().cxh,
                    yxtzjid = group.First().yxtzjid,
                    lch = group.Key,
                    State = group.Any(dto => dto.State == "2") ? "2" :
                       (group.Any(dto => dto.State == "1") ? "1" : "0") 
                }).ToList();
            }
            else
            {
                result = data.GroupBy(x => x.lch).Select(group => new TrainStaDTO
                {
                    lch = group.Key,
                    State = group.Any(dto => dto.State == "2") ? "2" :
                      (group.Any(dto => dto.State == "1") ? "1" : "0") // ���û��1��2����Ĭ��Ϊ���ַ�������ָ����ֵ  
                }).ToList();
            }

            resultData.Data = result;
            return resultData;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGroupByTrain")]
        public async Task<AjaxResult<List<LchGroup>>> GetTrain(string lch)
        {
            var result = new AjaxResult<List<LchGroup>>();
            string sql = @"SELECT
	                            s.lch, 
	                            s.cxh, 
	                            s.device_code, 
	                            s.yxtzjid, 
	                            s.State
                            FROM
	                            dbo.TB_PARSING_NEWDATAS AS s";

            var data = await _db.SqlQueryable<TrainStaDTO>(sql).ToListAsync();

            if (!string.IsNullOrEmpty(lch))
            {
                data = data.Where(a => a.lch == lch).ToList();
            }

            var group = data.GroupBy(x => x.lch)
                .Select(g => new LchGroup(g.Key)
                {
                    CxhGroups = g.GroupBy(x => x.cxh)
                    .Select(cg => new CxhGroup(cg.Key)
                    {
                        YxtzjidList = cg.Select(dto => new JZ() { Jz = dto.yxtzjid, JzName = dto.yxtzjid == "1" ? "����1" : "����2" }).ToList()
                    }).ToList()
                }).ToList();

            result.Data = group;
            return result;

        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("InitTables")]
        public AjaxResult<string> InitTables()
        {
            _db.CodeFirst.InitTables<FaultOrWarn>();
            return new AjaxResult<string>()
            {
                Message = "��ṹ��ʼ���ɹ�"
            };
        }


        /// <summary>
        /// ��ȡ�ؼ����ݷ�ҳ��Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetParesData")]
        public async Task<PageResult<TB_PARSING_DATAS>> GetParesData(string? lch,string? cxh, string? jz ,string sortFile = "create_time", string sortType = "desc", int pageIndex = 1, int pageRow = 20) 
        {
            var data = DateTime.Now.ToString("yyyyMMdd");
            var sql = $@"select * from TB_PARSING_DATAS_{data}";
            var q =  _db.SqlQueryable<TB_PARSING_DATAS>(sql);

            if (!string.IsNullOrEmpty(lch))
            {
                q = q.Where(a => a.lch == lch);
            }
            if (!string.IsNullOrEmpty(cxh))
            {
                q = q.Where(a => a.cxh == cxh);
            }
            if (!string.IsNullOrEmpty(jz))
            {
                q = q.Where(a => a.yxtzjid == Convert.ToInt32(jz));
            }

            return await q.GetPageResultAsync(sortFile, sortType, 1, 20);

        }

        /// <summary>
        /// ��ȡ��ҳ�豸����(����)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNEWDATAS")]
        public async Task<PageResult<TB_PARSING_NEWDATAS>> GetNEWDATAS()
        {
            var config = await _db.Queryable<SYS_Config>().Where(x => x.concode == "FaultSj").FirstAsync();
            string sql1 = @"SELECT
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
									dbo.TB_PARSING_DATAS" + $"_{DateTime.Now.ToString("yyyyMMdd")} " + "AS pd  " +
                                $@"WHERE
									pd.create_time >= DATEADD(MINUTE,-{config.conval},GETDATE())";

            var sql = @"select * from TB_PARSING_NEWDATAS";

            var data = _db.SqlQueryable<TB_PARSING_NEWDATAS>(sql);
            var fault = _db.Queryable<FaultOrWarn>().ToList();

            return await data.GetPageResultAsync("create_time", "desc", 1, 20);
        }

        /// <summary>
        /// ��ȡ��������ƽ��ֵ
        /// </summary>
        /// <param name="cxh">�����</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTrainCarriage")]
        public async Task<AjaxResult<TrainCarriageDTO>> GetTrainCarriage(string cxh)
        {
            string where = "";
            if (!string.IsNullOrEmpty(cxh))
            {
                where += $"and cxh={cxh}";
            }

            var dic = await _db.Queryable<TB_SYS_DIC>().Where(x => x.ParentId == "1035").ToListAsync();

            var result = new AjaxResult<TrainCarriageDTO>();
            var data = new TrainCarriageDTO();
            string sql = $@"select jz1kswd,jz1swwd,jz1mbwd,kssdz,jz1co2nd,tfms,yxtzjid,cxh
                              from TB_PARSING_NEWDATAS
                              where cxh = '{cxh}'";

            var q = await _db.SqlQueryable<TrainCarriageDTO>(sql).ToListAsync();
            var num = q.Count();

            for (int i = 0; i < num; i++)
            {
                
                data.jz1kswd += q[i].jz1kswd;
                data.jz1swwd += q[i].jz1swwd;
                data.jz1mbwd += q[i].jz1mbwd;
                data.jz1co2nd += q[i].jz1co2nd;
                data.kssdz += q[i].kssdz;
                if (q[i].yxtzjid == 1)
                {
                    data.jz1tfms = q[i].tfms;
                    data.jz1tfmsName = dic.Where(x => x.Code == q[i].tfms.ToString()).First().Name;
                }
                if (q[i].yxtzjid == 2)
                {
                    data.jz2tfms = q[i].tfms;
                    data.jz2tfmsName = dic.Where(x => x.Code == q[i].tfms.ToString()).First().Name;
                }
            }

            data.jz1kswd = Math.Round(data.jz1kswd / num, 1);
            data.jz1swwd = Math.Round(data.jz1swwd / num, 1);
            data.jz1mbwd = Math.Round(data.jz1mbwd / num, 1);
            data.jz1co2nd = Math.Round(data.jz1co2nd / num, 1);
            data.kssdz = Math.Round(data.kssdz / num, 1);
            data.cxh = q.First().cxh;
            result.Data = data;

            return result;
        }



        /// <summary>
        /// ��ȡ����ͼ����
        /// </summary>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <param name="code">�ؼ�����Code����ѡ�ã��ָ�</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("GetTrainLine")]
        public async Task<AjaxResult<List<TrainCarriageDTO>>> GetTrainLine(string cxh, string? jz, string? code,string? startTime, string? endTime)
        {
            var result = new AjaxResult<List<TrainCarriageDTO>>();

            string cloum = "";
            if (string.IsNullOrEmpty(cxh))
            {
                throw new Exception("����Ų���Ϊ��");
            }
            string wheresql = "";

            if (!string.IsNullOrEmpty(jz))
            {
                wheresql += " and yxtzjid = " + jz + "";
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                wheresql += " and create_time >= '" + startTime + "'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                wheresql += " and create_time < '" + endTime + "'";
            }

            if (!string.IsNullOrEmpty(code))
            {
                cloum = code;
            }

            string sql = "select jz1kswd,kssdz,jz1co2nd,create_time,yxtzjid,cxh " +
                        " from TB_PARSING_DATAS" + $"_{DateTime.Now.ToString("yyyyMMdd")} " +
                        " where cxh='{0}'{1}" +
                        " order by create_time";
            sql = string.Format(sql, cxh, wheresql);

            var data = await _db.SqlQueryable<TrainCarriageDTO>(sql).ToListAsync();

            var group = data.GroupBy(x => (Convert.ToDateTime(x.create_time).ToString("yyyy-MM-dd HH:mm"))).Select(g => new TrainCarriageDTO
            {
                cxh = g.FirstOrDefault()?.cxh,
                jz1kswd = Math.Round(g.Average(x => x.jz1kswd),1),
                kssdz = Math.Round(g.Average(x => x.kssdz),1),
                jz1co2nd = Math.Round(g.Average(x => x.jz1co2nd),1),
                create_time = g.Key

            }).ToList();

            result.Data = group;
            return result;
        }


        /// <summary>
        /// ͨ�����Ի�ȡ����ͼ����
        /// </summary>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <param name="code">�ؼ�����Code����ѡ�ã��ָ�</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route("GetTrainLineInfo")]
        public async Task<AjaxResult<List<TB_PARSING_NEWDATAS>>> GetTrainLineInfo(string cxh, string? jz, string? startTime, string? endTime, string? code = "jz1kswd,kssdz,jz1co2nd")
        {
            var result = new AjaxResult<List<TB_PARSING_NEWDATAS>>();

            if (string.IsNullOrEmpty(cxh))
            {
                throw new Exception("����Ų���Ϊ��");
            }

            string wheresql = "";

            if (!string.IsNullOrEmpty(jz))
            {
                wheresql += " and yxtzjid = " + jz + "";
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                wheresql += " and create_time >= '" + startTime + "'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                wheresql += " and create_time < '" + endTime + "'";
            }

            string sql = $"select lch,create_time,yxtzjid,cxh,{code} " +
                        " from TB_PARSING_DATAS" + $"_{DateTime.Now.ToString("yyyyMMdd")} " +
                        " where cxh='{0}'{1}" +
                        " order by create_time";

            if (string.IsNullOrEmpty(code))
            {
                sql = $"select * " +
                       " from TB_PARSING_DATAS" + $"_{DateTime.Now.ToString("yyyyMMdd")} " +
                       " where cxh='{0}'{1}" +
                       " order by create_time";
            }
            sql = string.Format(sql, cxh, wheresql);

            var data = await _db.SqlQueryable<TB_PARSING_NEWDATAS>(sql).ToListAsync();

            var group = data.GroupBy(x => (Convert.ToDateTime(x.create_time).ToString("yyyy-MM-dd HH:mm"))).Select(g => new TB_PARSING_NEWDATAS
            {
                cxh = g.First().cxh,
                jz1kswd = Math.Round(g.Average(x => x.jz1kswd),1),
                kssdz = (int)g.Average(x => x.kssdz),
                jz1co2nd = (int)g.Average(x => x.jz1co2nd),
                jz1mbwd = Math.Round(g.Average(y => y.jz1mbwd), 1),
                jz1swwd = Math.Round(g.Average(z => z.jz1swwd), 1),
                jz1kswdcgq1wd = Math.Round(g.Average(x => x.jz1kswdcgq1wd), 1),
                jz1sfcgq1wd = Math.Round(g.Average(x => x.jz1sfcgq1wd), 1),
                jz1sfcgq2wd = Math.Round(g.Average(x => x.jz1sfcgq2wd), 1),
                jz1ysj1pqwd = Math.Round(g.Average(x => x.jz1ysj1pqwd), 1),
                jz1ysj2pqwd = Math.Round(g.Average(x => x.jz1ysj2pqwd), 1),
                jz1ysj1xqwd = Math.Round(g.Average(x => x.jz1ysj1xqwd), 1),
                jz1ysj2xqwd = Math.Round(g.Average(x => x.jz1ysj2xqwd), 1),
                jz1kqzljcmkwd = Math.Round(g.Average(x => x.jz1kqzljcmkwd), 1),
                jz1pm2d5nd = (int)g.Average(x => x.jz1pm2d5nd),
                jz1tvocnd = (int)g.Average(x => x.jz1tvocnd),
                jz1xff1kd = (int)g.Average(x => x.jz1xff1kd),
                jz1xff2kd = (int)g.Average(x => x.jz1xff2kd),
                jz1hff1kd = (int)g.Average(x => x.jz1hff1kd),
                jz1hff2kd = (int)g.Average(x => x.jz1hff2kd),
                jz1ysj1gyyl = (int)g.Average(x => x.jz1ysj1gyyl),
                jz1ysj1dyyl = (int)g.Average(x => x.jz1ysj1dyyl),
                jz1ysj2gyyl = (int)g.Average(x => x.jz1ysj2gyyl),
                jz1ysj2dyyl = (int)g.Average(x => x.jz1ysj2dyyl),
                jz1lwylz = (int)g.Average(x => x.jz1lwylz),
                jz1tfj1uxdlz = Math.Round(g.Average(x => x.jz1tfj1uxdlz), 1),
                jz1tfj1vxdlz = Math.Round(g.Average(x => x.jz1tfj1vxdlz), 1),
                jz1tfj1wxdlz = Math.Round(g.Average(x => x.jz1tfj1wxdlz), 1),
                jz1tfj2uxdlz = Math.Round(g.Average(x => x.jz1tfj2uxdlz), 1),
                jz1tfj2vxdlz = Math.Round(g.Average(x => x.jz1tfj2vxdlz), 1),
                jz1tfj2wxdlz = Math.Round(g.Average(x => x.jz1tfj2wxdlz), 1),
                jz1lnfj1uxdlz = Math.Round(g.Average(x => x.jz1lnfj1uxdlz), 1),
                jz1lnfj1vxdlz = Math.Round(g.Average(x => x.jz1lnfj1vxdlz), 1),
                jz1lnfj1wxdlz = Math.Round(g.Average(x => x.jz1lnfj1wxdlz), 1),
                jz1lnfj2uxdlz = Math.Round(g.Average(x => x.jz1lnfj2uxdlz), 1),
                jz1lnfj2vxdlz = Math.Round(g.Average(x => x.jz1lnfj2vxdlz), 1),
                jz1lnfj2wxdlz = Math.Round(g.Average(x => x.jz1lnfj2wxdlz), 1),
                jz1ysj1uxdlz = Math.Round(g.Average(x => x.jz1ysj1uxdlz), 1),
                jz1ysj1vxdlz = Math.Round(g.Average(x => x.jz1ysj1vxdlz), 1),
                jz1ysj1wxdlz = Math.Round(g.Average(x => x.jz1ysj1wxdlz), 1),
                jz1ysj2uxdlz = Math.Round(g.Average(x => x.jz1ysj2uxdlz), 1),
                jz1ysj2vxdlz = Math.Round(g.Average(x => x.jz1ysj2vxdlz), 1),
                jz1ysj2wxdlz = Math.Round(g.Average(x => x.jz1ysj2wxdlz), 1),
                jz1ysj1pl = (int)g.Average(x => x.jz1ysj1pl),
                jz1ysj2pl = (int)g.Average(x => x.jz1ysj2pl),
                jz1bpq1gl = Math.Round(g.Average(x => x.jz1bpq1gl),1),
                jz1bpq2gl = Math.Round(g.Average(x => x.jz1bpq2gl), 1),
                jz1bpq1scdy = Math.Round(g.Average(x => x.jz1bpq1scdy), 1),
                jz1bpq2scdy = Math.Round(g.Average(x => x.jz1bpq2scdy), 1),
                jz1ktnh = (int)g.Average(x => x.jz1ktnh),
                create_time = Convert.ToDateTime(g.Key)

            }).ToList();

            result.Data = group;
            return result;
        }

        /// <summary>
        /// ����ͼ���ݵ���
        /// </summary>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="code">�ؼ�����</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetLineData/excel")]
        public async Task<IActionResult> LineDataExcel(string cxh, string? jz, string? startTime, string? endTime, string? code = "jz1kswd,kssdz,jz1co2nd")
        {
            try
            {
                string rptTitle = "����ͼ���ݵ���";
                ExcelUtil.Instance().FileName = $"{rptTitle}.xlsx";
                ExcelUtil.Instance().AliasDataSource.Clear();
                var data = await GetTrainLineInfo(cxh, jz, startTime, endTime, code);
                var dataTable = data.Data.ToDataTable(); // ȷ�� ToDataTable() �������ڻ���ȷʵ��  
                dataTable.TableName = "LineData";
                //{ rptTitle}
                //_{ DateTime.Now}

                // ���� Save ������ȡ MemoryStream  
                using (var stream = ExcelUtil.Instance().Save(dataTable, "����ͼ���ݵ���ģ��"))
                {
                    byte[] excelBytes = new byte[stream.Length];
                    stream.Read(excelBytes, 0, excelBytes.Length);

                    // �����ļ���  
                    string fileName = rptTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                    // �����ļ����ͻ���  
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
            catch (Exception ex)
            {
                // ��¼��������쳣  
                // ���磬����ʹ����־��¼����¼�쳣  
                _logger.LogError(ex, "Error occurred while generating Excel file.");

                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// ��ȡ�յ�����ؼ�����
        /// </summary>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceData")]
        public async Task<AjaxResult<TrainKeyDTO>> GetDeviceData(string cxh, string? jz)
        {
            var result = new AjaxResult<TrainKeyDTO>();
            string sql = @"SELECT
	                        a.lch, 
	                        a.cxh, 
	                        a.yxtzjid, 
	                        a.jz1mbwd, 
	                        a.jz1kswd, 
	                        a.jz1swwd, 
	                        a.jz1kswdcgq1wd, 
	                        a.jz1sfcgq1wd, 
	                        a.jz1sfcgq2wd, 
	                        a.jz1ysj1pqwd, 
	                        a.jz1ysj2pqwd, 
	                        a.jz1ysj1xqwd, 
	                        a.jz1ysj2xqwd, 
	                        a.jz1kqzljcmkwd, 
	                        a.jz1co2nd, 
	                        a.jz1pm2d5nd, 
	                        a.kssdz, 
	                        a.jz1tvocnd, 
	                        a.jz1kssdz, 
	                        a.jz1xff1kd, 
	                        a.jz1xff2kd, 
	                        a.jz1hff1kd, 
	                        a.jz1hff2kd, 
	                        a.jz1ysj1gyyl, 
	                        a.jz1ysj1dyyl, 
	                        a.jz1ysj2gyyl, 
	                        a.jz1ysj2dyyl, 
	                        a.jz1lwylz, 
	                        a.jz1tfj1uxdlz, 
	                        a.jz1tfj1vxdlz, 
	                        a.jz1tfj1wxdlz, 
	                        a.jz1tfj2uxdlz, 
	                        a.jz1tfj2vxdlz, 
	                        a.jz1tfj2wxdlz, 
	                        a.jz1lnfj1uxdlz, 
	                        a.jz1lnfj1vxdlz, 
	                        a.jz1lnfj1wxdlz, 
	                        a.jz1lnfj2uxdlz, 
	                        a.jz1lnfj2vxdlz, 
	                        a.jz1lnfj2wxdlz, 
	                        a.jz1ysj1uxdlz, 
	                        a.jz1ysj1vxdlz, 
	                        a.jz1ysj1wxdlz, 
	                        a.jz1ysj2uxdlz, 
	                        a.jz1ysj2vxdlz, 
	                        a.jz1ysj2wxdlz, 
	                        a.jz1ysj1pl, 
	                        a.jz1ysj2pl, 
	                        a.jz1bpq1gl, 
	                        a.jz1bpq2gl, 
	                        a.jz1bpq1scdy, 
	                        a.jz1bpq2scdy, 
	                        a.jz1ktnh, 
	                        a.jz1zhl1ldlz, 
	                        a.jz1zhl2ldlz, 
	                        a.ktkzms, 
	                        a.tfms, 
	                        dic.Name AS tfmsName
                        FROM
	                        dbo.TB_PARSING_NEWDATAS AS a
	                        INNER JOIN
	                        dbo.TB_SYS_DIC AS dic
	                    ON 
		                    a.tfms = dic.Code
                        WHERE
	                        dic.ParentId = '1035' AND
	                        a.cxh = '{0}' AND
	                        a.yxtzjid = {1}";
            sql = string.Format(sql, cxh, jz);

            var data = await _db.SqlQueryable<TrainKeyDTO>(sql).FirstAsync();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// ��ȡ������ҳ��Ϣ
        /// </summary>
        /// <param name="lch">�г���</param>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <param name="name">������������</param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex">ҳ��</param>
        /// <param name="pageRow">ÿҳ����</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPartsLife")]
        public async Task<PageResult<PartsLifeDTO>> GetPartsLife(string? lch, string? cxh, string? jz, string? name, string? gzbj, string sortFile = "Percent", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            var q = _db.Queryable<PartsLife>();

            if (!string.IsNullOrEmpty(lch))
            {
                q = q.Where(a => a.CH == lch);
            }
            if (!string.IsNullOrEmpty(cxh))
            {
                q = q.Where(a => a.CX == cxh);
            }
            if (!string.IsNullOrEmpty(jz))
            {
                q = q.Where(a => a.WZ == Convert.ToInt32(jz));
            }
            if (!string.IsNullOrEmpty(name))
            {               
                q = q.Where(a => a.Name == name);
            }
            if (!string.IsNullOrEmpty(gzbj))
            {
                var dicname = _db.Queryable<TB_SYS_DIC>().Where(x => x.ParentId == "1001" && x.Code == gzbj).First().Name;

                q = q.Where(a => a.Name == dicname);
            }

            var data = await q.Select(x => new PartsLifeDTO
            {
                Id = x.Id,
                Name = x.Name,
                XL = x.XL,
                CH = x.CH,
                CX = x.CX,
                WZ = x.WZ,
                WZName = x.WZ == 1 ? "����1" : "����2",
                Type = x.Type,
                TypeName = x.Type == "H" ? "ʱ��(Сʱ)" : "����",
                RunLife = x.RunLife,
                RatedLife = x.RatedLife,
                SurplusLife = (x.RatedLife) - (x.RunLife ?? 0),
                //Percent = (decimal?)((x.RunLife ?? 0)/(decimal?)x.RatedLife)

            }).ToListAsync();

            int count = await q.CountAsync();

            foreach (var item in data)
            {
                if (item.RatedLife != 0) // ����RatedLife��ӦΪ0�Ա�����������  
                {
                    item.Percent = Math.Round((decimal)((item.RunLife * 100) / item.RatedLife), 1);
                    item.RunLife = Math.Round((decimal)item.RunLife, 1);
                }
            }

            // ���Ӳ���ҳ�ж�
            if (pageRow <= 0)
            {
                pageRow = count;
            }

            data = data.OrderByDescending(x => x.Percent)
              .Skip((pageIndex - 1) * pageRow)
              .Take(pageRow)
              .ToList();

            //var result = await data.GetPageResultAsync(sortFile, sortType, pageIndex, pageRow);
            return new PageResult<PartsLifeDTO>
            {
                Data = data,
                Total = count,
                Success = true,
                Code = 200,
                Message = "��ѯ�ɹ�"
            };
        }

        /// <summary>
        /// ��ȡ����Ԥ����Ϣ
        /// </summary>
        /// <param name="lch">�г���</param>
        /// <param name="cxh">�����</param>
        /// <param name="jz">����</param>
        /// <param name="type">����1�����ϣ�2��Ԥ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex">ҳ��</param>
        /// <param name="pageRow">ÿҳ����</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaultWarn")]
        public async Task<PageResult<FaultPageDTO>> GetFaultWarn(string? lch, string? cxh, string? jz,string? type,string? startTime,string? endTime, string sortFile = "createtime", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {        
            string sql = @"SELECT
                                f.Id,
	                            f.Type, 
	                            f.State, 
	                            f.createtime, 
	                            f.updatetime, 
	                            f.xlh, 
	                            e.FaultName, 
	                            e.FaultCode, 
	                            e.DriverSln, 
	                            e.Solution, 
	                            f.lch, 
	                            f.cxh, 
	                            e.HvacType, 
	                            s_dic.Name AS StateName, 
	                            t_dic.Name AS TypeName, 
	                            h_dic.Name AS HvacTypeName
                            FROM
	                            dbo.FaultOrWarn AS f
	                            LEFT JOIN
	                            dbo.EquipmentFault AS e
	                            ON 
		                            f.Code = e.FaultCode
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS h_dic
	                            ON 
		                            e.HvacType = h_dic.Code
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS t_dic
	                            ON 
		                            f.Type = t_dic.Code
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS s_dic
	                            ON 
		                            f.State = s_dic.Code
                            WHERE
	                            t_dic.ParentId = '10000' AND
	                            s_dic.ParentId = '10008' AND
	                            h_dic.ParentId = '10003'
                           ";

            var q =  _db.SqlQueryable<FaultPageDTO>(sql);

            if (!string.IsNullOrEmpty(lch))
            {
                q = q.Where(a => a.lch == lch);
            }
            if (!string.IsNullOrEmpty(cxh))
            {
                q = q.Where(a => a.cxh == cxh);
            }
            if (!string.IsNullOrEmpty(jz))
            {
                q = q.Where(a => a.HvacType == Convert.ToInt32(jz));
            }
            if (!string.IsNullOrEmpty(type))
            {
                q = q.Where(a => a.Type == type);
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                startTime  += " 00:00:00";
                q = q.Where(a => Convert.ToDateTime(a.createtime) >= Convert.ToDateTime(startTime));
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                endTime += " 23:59:59";
                q = q.Where(a => Convert.ToDateTime(a.createtime) <= Convert.ToDateTime(endTime));
            }

            var result = await q.GetPageResultAsync(sortFile, sortType, pageIndex, pageRow);

            return result;
        }

        /// <summary>
        /// ��ȡ����Ԥ����ϸ��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaultWarn/info")]
        public async Task<AjaxResult<FaultPageDTO>> GetFaultWarnInfo(string id)
        {
            var result = new AjaxResult<FaultPageDTO>();
            string sql = $@"SELECT
                                f.Id,
	                            f.Type, 
	                            f.State, 
	                            f.createtime, 
	                            f.updatetime, 
	                            f.xlh, 
	                            e.FaultName, 
	                            e.FaultCode, 
	                            e.DriverSln, 
	                            e.Solution, 
	                            f.lch, 
	                            f.cxh, 
	                            e.HvacType, 
	                            s_dic.Name AS StateName, 
	                            t_dic.Name AS TypeName, 
	                            h_dic.Name AS HvacTypeName
                            FROM
	                            dbo.FaultOrWarn AS f
	                            LEFT JOIN
	                            dbo.EquipmentFault AS e
	                            ON 
		                            f.Code = e.FaultCode
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS h_dic
	                            ON 
		                            e.HvacType = h_dic.Code
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS t_dic
	                            ON 
		                            f.Type = t_dic.Code
	                            LEFT JOIN
	                            dbo.TB_SYS_DIC AS s_dic
	                            ON 
		                            f.State = s_dic.Code
                            WHERE
	                            t_dic.ParentId = '10000' AND
	                            s_dic.ParentId = '10008' AND
	                            h_dic.ParentId = '10003' AND
                                f.Id = {id}
                           ";

            var q = await _db.SqlQueryable<FaultPageDTO>(sql).FirstAsync();

            result.Data = q;

            return result;           
        }

        /// <summary>
        /// ����Ԥ������excel
        /// </summary>
        /// <param name="lch"></param>
        /// <param name="cxh"></param>
        /// <param name="jz"></param>
        /// <param name="type"></param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFaultWarn/excel")]
        public async Task<IActionResult> FaultWarnExcel(string? lch, string? cxh, string? jz, string? type, string? startTime, string? endTime, string sortFile = "createtime", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            try
            {
                string rptTitle = "����Ԥ������ģ��";
                ExcelUtil.Instance().FileName = $"{rptTitle}.xlsx";
                ExcelUtil.Instance().AliasDataSource.Clear();
                var data = await GetFaultWarn(lch, cxh, jz, type, startTime, endTime, sortFile, sortType,pageIndex, pageRow);
                var dataTable = data.Data.ToDataTable(); // ȷ�� ToDataTable() �������ڻ���ȷʵ��  
                dataTable.TableName = "FaultWarn";
                //{ rptTitle}
                //_{ DateTime.Now}

                // ���� Save ������ȡ MemoryStream  
                using (var stream = ExcelUtil.Instance().Save(dataTable, "����Ԥ������ģ��"))
                {

                    byte[] excelBytes = new byte[stream.Length];
                    stream.Read(excelBytes, 0, excelBytes.Length);

                    // �����ļ���  
                    string fileName = rptTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                    // �����ļ����ͻ���  
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
            catch (Exception ex)
            {
                // ��¼��������쳣  
                // ���磬����ʹ����־��¼����¼�쳣  
                _logger.LogError(ex, "Error occurred while generating Excel file.");

                // ����һ��������Ӧ  
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the Excel file.");
            }
        }


        /// <summary>
        /// ��ȡ����Ԥ��������Ϣ
        /// </summary>
        /// <param name="lch">�г���</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaultWarnType")]
        public async Task<AjaxResult<List<FaultTypeDTO>>> GetFaultWarnType(string? lch, string? type,string? timeType, string? startTime, string? endTime)
        {

            string wheresql = "";

            if (!string.IsNullOrEmpty(lch))
            {
                wheresql += " and f.lch = " + lch + "";
            }

            if (!string.IsNullOrEmpty(type))
            {
                wheresql += " and f.Type = " + type + "";
            }
            if (!string.IsNullOrEmpty(timeType))
            {
                var nowDate = DateTime.Now;
                endTime = nowDate.ToString("yyyy-MM-dd HH:mm:ss");

                switch (timeType)
                {
                    case "0":                        
                        startTime = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "1":
                        startTime = nowDate.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "3":
                        startTime = nowDate.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "6":
                        startTime = nowDate.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    default:
                        throw new Exception("δ֪��ʱ������");
                }
                wheresql += " and f.createtime >= '" + startTime + "'" + " and f.createtime < '" + endTime + "'";

            }
            else
            {
                if (!string.IsNullOrEmpty(startTime))
                {
                    wheresql += " and f.createtime >= '" + startTime + "'";
                }

                if (!string.IsNullOrEmpty(endTime))
                {
                    wheresql += " and f.createtime < '" + endTime + "'";

                }
            }


            string sql = $@"SELECT
	                                f.lch, 
	                                f.FaultType AS TypeCode, 
	                                f.createtime, 
	                                dic.Name AS TypeName, 
	                                f.Type
                                FROM
	                                dbo.FaultOrWarn AS f
	                                INNER JOIN
	                                dbo.TB_SYS_DIC AS dic
	                                ON 
		                                f.FaultType = dic.Code
                                WHERE 
                                      1=1{wheresql}
                           ";

            var q = await _db.SqlQueryable<FaultTypeDTO>(sql).ToListAsync();

            var data = q.GroupBy(x => new { x.TypeCode, x.TypeName }).Select(x => new FaultTypeDTO
            {
                lch = x.FirstOrDefault()?.lch,
                Type = type,
                TypeCode = x.Key.TypeCode,
                TypeName = x.Key.TypeName,
                TypeNum = x.Count()
            }).OrderByDescending(x => x.TypeNum).ToList();

            var result = new AjaxResult<List<FaultTypeDTO>>();
            result.Data = data;

            return result;

        }

        /// <summary>
        /// ��ȡ�յ�״̬����
        /// </summary>
        /// <param name="lch"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFaultWarnNum")]
        public async Task<AjaxResult<FaultWarnNum>> GetFaultWarnNum(string? lch)
        {
            var result = new AjaxResult<FaultWarnNum>();
            var q = await _db.Queryable<FaultOrWarn>().Where(x => Convert.ToDateTime(x.createtime).Date == DateTime.Now.Date).ToListAsync();

            var num = 228;
            if (!string.IsNullOrEmpty(lch))
            {
                q = q.Where(x => x.lch == lch).ToList();
                num = 12;
            }

            var warnNum = q.Where(x => x.Type == "2" );
            var faultNum = q.Where(x => x.Type == "1");

            var fault = new FaultWarnNum() 
            { 
                SubHealthNum = warnNum.Where(x => x.State == "0").Select(x => x.DeviceCode).Distinct().Count(),
                UnHealthNum = faultNum.Where(x => x.State == "0").Select(x => x.DeviceCode).Distinct().Count(),
                WarnSum = warnNum.Count(),
                FaultSum = faultNum.Count(),
            };

            fault.HealthNum = num - fault.SubHealthNum - fault.UnHealthNum;
            result.Data = fault;
            return result;
         
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="lch"></param>
        /// <param name="cxh"></param>
        /// <param name="jz"></param>
        /// <param name="name"></param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPartsLife/excel")]
        public async Task<IActionResult> PartsLifeExcel(string? lch, string? cxh, string? jz, string? name, string? gzbj, string sortFile = "Percent", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            try
            {
                string rptTitle = "������������";
                ExcelUtil.Instance().FileName = $"{rptTitle}.xlsx";
                ExcelUtil.Instance().AliasDataSource.Clear();
                var data = await GetPartsLife(lch, cxh, jz, name, gzbj,sortFile, sortType, pageIndex, pageRow);
                var dataTable = data.Data.ToDataTable(); // ȷ�� ToDataTable() �������ڻ���ȷʵ��  
                dataTable.TableName = "PartsLife";
                //{ rptTitle}
                //_{ DateTime.Now}

                // ���� Save ������ȡ MemoryStream  
                using (var stream = ExcelUtil.Instance().Save(dataTable, "������������ģ��"))
                {

                    byte[] excelBytes = new byte[stream.Length];
                    stream.Read(excelBytes, 0, excelBytes.Length);

                    // �����ļ���  
                    string fileName = rptTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                    // �����ļ����ͻ���  
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
            catch (Exception ex)
            {
                // ��¼��������쳣  
                // ���磬����ʹ����־��¼����¼�쳣  
                _logger.LogError(ex, "Error occurred while generating Excel file.");

                // ����һ��������Ӧ  
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the Excel file.");
            }
        }
        /// <summary>
        /// �ؼ����ݵ���
        /// </summary>
        /// <param name="lch"></param>
        /// <param name="cxh"></param>
        /// <param name="jz"></param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetParesData/excel")]
        public async Task<IActionResult> ParesDataExcel(string? lch, string? cxh, string? jz, string sortFile = "create_time", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            try
            {
                string rptTitle = "�ؼ����ݵ���";
                ExcelUtil.Instance().FileName = $"{rptTitle}.xlsx";
                ExcelUtil.Instance().AliasDataSource.Clear();
                var data = await GetParesData(lch, cxh, jz, sortFile, sortType, pageIndex, pageRow);
                var dataTable = data.Data.ToDataTable(); // ȷ�� ToDataTable() �������ڻ���ȷʵ��  
                dataTable.TableName = "ParesData";
                //{ rptTitle}
                //_{ DateTime.Now}

                // ���� Save ������ȡ MemoryStream  
                using (var stream = ExcelUtil.Instance().Save(dataTable, "�ؼ����ݵ���ģ��"))
                {

                    byte[] excelBytes = new byte[stream.Length];
                    stream.Read(excelBytes, 0, excelBytes.Length);

                    // �����ļ���  
                    string fileName = rptTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                    // �����ļ����ͻ���  
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
            catch (Exception ex)
            {
                // ��¼��������쳣  
                // ���磬����ʹ����־��¼����¼�쳣  
                _logger.LogError(ex, "Error occurred while generating Excel file.");

                // ����һ��������Ӧ  
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the Excel file.");
            }
        }
    }
}

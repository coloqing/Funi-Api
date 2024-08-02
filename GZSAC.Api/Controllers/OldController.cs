using GZSAC.Api.DTO.OldDTO;
using GZSAC.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAC.Util.DTO;
using SqlSugar;
using System.Collections;
using System.Data;

namespace GZSAC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldController : ControllerBase
    {
        private readonly SqlSugarClient _db;

        private readonly AppSettings _appSettings;

        private readonly ILogger<OldController> _logger;

        public OldController(
            ILogger<OldController> logger,
            SqlSugarClient db,
            AppSettings appSettings)
        {
            _logger = logger;
            _db = db;
            _appSettings = appSettings;
        }

        /// <summary>
        /// 获取多设备参数折线图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDevsCsLine")]
        public JsonMain GetDevsCsLine(DeviceModels model)
        {
            JsonMain jsonMain = new JsonMain();
            jsonMain.Status = JsonStatus.error;
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                string where = "";
                string tablename = "";
                if (!string.IsNullOrEmpty(model.startime))
                {
                    where += " and create_time >= '" + model.startime + "'";
                    //if (model.startime.Substring(0, 10) == DateTime.Now.ToString("yyyy-MM-dd"))
                    //{
                    //    tablename = "TB_PARSING_DATAS";
                    //}
                    //else
                    {
                        tablename = "TB_PARSING_DATAS_" + model.startime.Substring(0, 10).Replace("-", "");
                    }

                }
                else
                {
                    where += " and create_time >= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
                if (!string.IsNullOrEmpty(model.endtime))
                {
                    where += " and create_time < '" + model.endtime + "'";
                }
                string csids = "";
                if (!string.IsNullOrEmpty(model.devcsids))
                {
                    csids = model.devcsids.Replace(";", ",");
                }
                //获取参数信息
                string csval = "";
                string csname = "";
                if (!string.IsNullOrEmpty(csids))
                {
                    var res = SelDevCsData(csids);
                    if (res.Count > 0)
                    {
                        csval = res["csval"].ToString();
                        csname = res["csname"].ToString();
                    }
                }
                else
                {
                    result.Add("data", null);
                    jsonMain.data = result;
                    return jsonMain;
                }

                //分组
                var csmc = csname.Split(',');
                var csvl = csval.Split(',');
                //获取设备信息
                var des = model.device_id.Split(',');
                //定义容器
                ArrayList data = new ArrayList();
                //循环设备
                for (int a = 0; a < des.Length; a++)
                {
                    string devid = des[a];
                    if (!string.IsNullOrEmpty(devid))
                    {
                        //查询设备参数集合数据
                        string sql = @"select CONVERT(varchar(100), create_time, 20) AS cjsj," + csval + " from " + tablename + " where cxh = '" + devid + "' " + where + @"  order by rq";
                        DataTable dt = _db.Ado.GetDataTable(sql);
                        //循环参数
                        for (int b = 0; b < csvl.Length; b++)
                        {
                            Dictionary<string, object> rest = new Dictionary<string, object>();
                            ArrayList list = new ArrayList();
                            string devcsval = csvl[b];
                            string devcsname = csmc[b];
                            //循环数据
                            for (int t = 0; t < dt.Rows.Count; t++)
                            {
                                string cjsj = dt.Rows[t]["cjsj"].ToString();
                                string csz = dt.Rows[t][devcsval].ToString();
                                string[] may = { cjsj, csz };
                                list.Add(may);
                            }
                            rest.Add("csmc", devid + devcsname);
                            rest.Add("data", list);
                            data.Add(rest);
                        }
                    }
                }

                result.Add("data", data);

                jsonMain.data = result;
                jsonMain.Status = JsonStatus.success;
            }
            catch (Exception ex)
            {
                jsonMain.Msg = ex.Message;
            }
            return jsonMain;
        }

        /// <summary>
        /// 获取设备参数信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SelDevCsData")]
        public Dictionary<string, object> SelDevCsData(string ids)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                string csval = "";
                string csname = "";
                string where = "";
                if (!string.IsNullOrEmpty(ids))
                {
                    where += " and id in (" + ids + ")";
                }
                string sql = "select * from DEVCSXD where csval != '' " + where + "";
                DataTable dt = _db.Ado.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int a = 0; a < dt.Rows.Count; a++)
                    {
                        csval += dt.Rows[a]["csval"].ToString().Replace("-", "_") + ",";
                        csname += dt.Rows[a]["csname"] + ",";
                    }
                    csval = csval.Substring(0, csval.Length - 1);
                    csname = csname.Substring(0, csname.Length - 1);
                }
                res.Add("csval", csval);
                res.Add("csname", csname);
            }
            catch (Exception)
            { }
            return res;
        }

        /// <summary>
        /// 获取所属树结构接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTreeByGZ22")]
        public JsonMain GetTreeByGZ22(string kslevnum, string jslevnum)
        {
            JsonMain jsonMain = new JsonMain();
            jsonMain.Status = JsonStatus.error;
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(kslevnum))
                {
                    where += " and levnum >= " + kslevnum;
                }
                if (!string.IsNullOrEmpty(jslevnum))
                {
                    where += " and levnum <= " + jslevnum;
                }
                string sql = @"	with tree as 
                                     (
                                     select * from allorg where 1=1 
                                     --and zid='LINE-22'可能是以前的人家的参数现在去掉
                                     union all select a.* from allorg as a,tree as b where b.zid=a.fid  --获取上级
                                     ),ptree as 
                                     (
                                     select * from allorg where 1=1 
                                     --and zid='LINE-22'可能是以前的人家的参数现在去掉
                                     union all select a.* from allorg as a,ptree as b where a.zid=b.fid --获取下级
                                     ) 
                                     select * from (
                                     select DISTINCT * from tree 
                                     UNION
                                     SELECT DISTINCT * FROM ptree  ) a  where levnum != 3 " + where + "";

                var reader = _db.Ado.GetDataReader(sql);
                var list = SystemService.DbReaderToHash(reader);
                reader.Close();
                List<Hashtable> o = SystemService.ArrayToTreeData(list, "zid", "fid") as List<Hashtable>;
                if (o.Count > 0)
                {
                    jsonMain.data = o;
                    jsonMain.Status = JsonStatus.success;
                }
            }
            catch (Exception ex)
            {
                jsonMain.Msg = ex.Message;
            }
            return jsonMain;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceCsList")]
        public JsonMain GetDeviceCsList(DevCsModels model)
        {
            JsonMain jsonMain = new JsonMain();
            jsonMain.Status = JsonStatus.error;
            try
            {
                string wheresql = "";
                if (!string.IsNullOrEmpty(model.csname))
                {
                    wheresql += " and (a.csname like '%" + model.csname + "%' or a.sxname like '%" + model.csname + "%')";
                }
                if (!string.IsNullOrEmpty(model.cswz))
                {
                    wheresql += " and a.cswz='" + model.cswz + "'";
                }
                if (!string.IsNullOrEmpty(model.zdxs))
                {
                    wheresql += " and a.zdxs='" + model.zdxs + "'";
                }
                if (!string.IsNullOrEmpty(model.sfqx))
                {
                    wheresql += " and a.csval != ''";
                }
                if (!string.IsNullOrEmpty(model.sfqx))
                {
                    wheresql += " and a.csval != ''";
                }
                if (!string.IsNullOrEmpty(model.average))
                {
                    wheresql += " and average = " + model.average + "";
                }
                string sql = @"SELECT a.*, b.username 
                                FROM devcsxd a
	                            LEFT JOIN SYS_USER b ON a.createuserid= b.userid where 1=1 " + wheresql + " order by a.sort";

                var dt = _db.Ado.GetDataTable(sql);
                jsonMain.data = dt;
                jsonMain.Status = JsonStatus.success;
            }
            catch (Exception ex)
            {
                jsonMain.Msg = ex.Message;
            }
            return jsonMain;
        }
    }
}

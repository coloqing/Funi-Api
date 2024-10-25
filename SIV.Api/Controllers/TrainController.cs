using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIV.Api.Authorization.Models;
using SIV.Api.DTO;
using SqlSugar;
using System.Diagnostics;
using System.Linq.Expressions;
using Util.DTO;
using SIV.Util;
using SIV.Api.Models;
using AutoMapper;
using Aspose.Cells.Charts;
using Microsoft.IdentityModel.Tokens;
using SIV.Entity;
using Newtonsoft.Json.Serialization;
using static Dm.net.buffer.ByteArrayBuffer;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public TrainController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TrainDTO, Train>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
          );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 获取列车状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("State")]
        public AjaxResult<List<TrainStateInfo>> GetState()
        {
            var result = new AjaxResult<List<TrainStateInfo>>();
#if DEBUG
            string json = "[{\"trainNum\":\"11001002\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11003004\",\"state\":1,\"alarm\":0,\"warning\":1},{\"trainNum\":\"11005006\",\"state\":-1,\"alarm\":8,\"warning\":0},{\"trainNum\":\"11007008\",\"state\":0,\"alarm\":5,\"warning\":3},{\"trainNum\":\"11009010\",\"state\":0,\"alarm\":7,\"warning\":5},{\"trainNum\":\"11011012\",\"state\":0,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11013014\",\"state\":1,\"alarm\":4,\"warning\":1},{\"trainNum\":\"11015016\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11017018\",\"state\":-1,\"alarm\":3,\"warning\":1},{\"trainNum\":\"11019020\",\"state\":-1,\"alarm\":0,\"warning\":8},{\"trainNum\":\"11021022\",\"state\":0,\"alarm\":4,\"warning\":3},{\"trainNum\":\"11023024\",\"state\":-1,\"alarm\":9,\"warning\":4},{\"trainNum\":\"11025026\",\"state\":1,\"alarm\":6,\"warning\":0},{\"trainNum\":\"11027028\",\"state\":1,\"alarm\":9,\"warning\":9},{\"trainNum\":\"11029030\",\"state\":0,\"alarm\":9,\"warning\":3},{\"trainNum\":\"11031032\",\"state\":-1,\"alarm\":4,\"warning\":3},{\"trainNum\":\"11033034\",\"state\":1,\"alarm\":6,\"warning\":6},{\"trainNum\":\"11035036\",\"state\":1,\"alarm\":5,\"warning\":5},{\"trainNum\":\"11037038\",\"state\":0,\"alarm\":0,\"warning\":1},{\"trainNum\":\"11039040\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11041042\",\"state\":1,\"alarm\":0,\"warning\":8},{\"trainNum\":\"11043044\",\"state\":1,\"alarm\":4,\"warning\":6},{\"trainNum\":\"11045046\",\"state\":-1,\"alarm\":2,\"warning\":0},{\"trainNum\":\"11047048\",\"state\":-1,\"alarm\":2,\"warning\":4},{\"trainNum\":\"11049050\",\"state\":1,\"alarm\":9,\"warning\":8},{\"trainNum\":\"11051052\",\"state\":1,\"alarm\":1,\"warning\":2},{\"trainNum\":\"11053054\",\"state\":1,\"alarm\":4,\"warning\":8},{\"trainNum\":\"11055056\",\"state\":0,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11057058\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11059060\",\"state\":-1,\"alarm\":1,\"warning\":5},{\"trainNum\":\"11061062\",\"state\":0,\"alarm\":1,\"warning\":0},{\"trainNum\":\"11063064\",\"state\":0,\"alarm\":9,\"warning\":6},{\"trainNum\":\"11065066\",\"state\":1,\"alarm\":4,\"warning\":9},{\"trainNum\":\"11067068\",\"state\":0,\"alarm\":9,\"warning\":2},{\"trainNum\":\"11069070\",\"state\":1,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11071072\",\"state\":0,\"alarm\":0,\"warning\":2},{\"trainNum\":\"11073074\",\"state\":1,\"alarm\":0,\"warning\":5},{\"trainNum\":\"11075076\",\"state\":0,\"alarm\":6,\"warning\":3},{\"trainNum\":\"11077078\",\"state\":-1,\"alarm\":2,\"warning\":4},{\"trainNum\":\"11079080\",\"state\":1,\"alarm\":8,\"warning\":4},{\"trainNum\":\"11081082\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11083084\",\"state\":-1,\"alarm\":6,\"warning\":1},{\"trainNum\":\"11085086\",\"state\":-1,\"alarm\":6,\"warning\":0},{\"trainNum\":\"11087088\",\"state\":1,\"alarm\":5,\"warning\":3},{\"trainNum\":\"11089090\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11091092\",\"state\":-1,\"alarm\":7,\"warning\":7},{\"trainNum\":\"11093094\",\"state\":0,\"alarm\":1,\"warning\":0},{\"trainNum\":\"11095096\",\"state\":-1,\"alarm\":2,\"warning\":0},{\"trainNum\":\"11097098\",\"state\":-1,\"alarm\":4,\"warning\":8},{\"trainNum\":\"11099100\",\"state\":0,\"alarm\":7,\"warning\":6}]";
            var data = JsonConvert.DeserializeObject<List<TrainStateInfo>>(json);

            result.Success = true;
            result.Data = data;
            return result;

#endif        

            result.Success = true;
            return result;
        }

        /// <summary>
        /// 获取列车在线报警预警统计
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet("TrainStateCount")]
        public AjaxResult<object> TrainStateCount(int? lineId = default)
        {
            var result = new AjaxResult<object>();
            result.Success = false;
            result.Code = 200;

            var exp = Expressionable.Create<Train>().And(x => !x.IsDeleted);

            if (lineId != default)
                exp.And(x => x.LineId == lineId);

            var query = sqlSugarClient.Queryable<Train>().Where(exp.ToExpression()).Count();

            var sData = GetState().Data;

            result.Data = new
            {
                Total = query,
                Online = sData.Where(x => x.state == 1).Sum(x => x.state),
                Normal = sData.Count(x => x.alarm == 0 && x.warning == 0),
                Alarm = sData.Count(x => x.alarm != 0),
                Warning = sData.Count(x => x.warning != 0),
            };

            return result;
        }

        [HttpGet]
        public async Task<PageResult<TrainDTO>> Get(int? lineId = default, int? pageIndex = 1, int? pageRow = 10)
        {
            var exp = Expressionable.Create<Train>().And(x => !x.IsDeleted);

            if (lineId != default)
                exp.And(x => x.LineId == lineId);

            var query = sqlSugarClient.Queryable<Train>().Where(exp.ToExpression());
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<TrainDTO>
            {
                Data = mapper.Map<List<TrainDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 列车添加
        /// </summary>
        /// <param name="train"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Add(TrainDTO train)
        {
            var result = new AjaxResult<string>();
            result.Success = false;
            result.Code = 200;

            var dbtrain = mapper.Map<Train>(train);

            dbtrain.CreateTime = DateTime.Now;
            dbtrain.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Insertable(dbtrain).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                return result;
            }

            return result;
        }


        /// <summary>
        /// 列车信息更新
        /// </summary>
        /// <param name="train"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(TrainDTO train)
        {
            var result = new AjaxResult<string>();
            result.Success = false;
            result.Code = 200;

            var dbtrain = sqlSugarClient.Queryable<Train>().Where(x => !x.IsDeleted && x.Id == train.Id).First();

            mapper.Map(train, dbtrain);

            dbtrain.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbtrain).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                return result;
            }

            return result;
        }

        /// <summary>
        /// 获取寿命分页信息
        /// </summary>
        /// <param name="lch">列车号</param>
        /// <param name="cxh">车厢号</param>
        /// <param name="jz">机组</param>
        /// <param name="name">寿命部件名称</param>
        /// <param name="sortFile"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageRow">每页行数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPartsLife")]
        public async Task<PageResult<PartsLifeDTO>> GetPartsLife(string? lch, string? cxh, string? jz, string? name, string? gzbj, string sortFile = "Percent", string sortType = "desc", int pageIndex = 1, int pageRow = 20)
        {
            var q = sqlSugarClient.Queryable<Models.PartsLife>();

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
            //if (!string.IsNullOrEmpty(gzbj))
            //{
            //    var dicname = sqlSugarClient.Queryable<TB_SYS_DIC>().Where(x => x.ParentId == "1001" && x.Code == gzbj).First().Name;
            //    q = q.Where(a => a.Name == dicname);
            //}

            var data = await q.Select(x => new PartsLifeDTO
            {
                Id = x.Id,
                Name = x.Name,
                XL = x.XL,
                CH = x.CH,
                CX = x.CX,
                WZ = x.WZ,
                WZName = x.WZ == 1 ? "机组1" : "机组2",
                Type = x.Type,
                TypeName = x.Type == "H" ? "时长(小时)" : "次数",
                RunLife = x.RunLife,
                RatedLife = x.RatedLife,
                SurplusLife = (x.RatedLife) - (x.RunLife ?? 0),
                UpdateTime = x.updatetime.Value
                //Percent = (decimal?)((x.RunLife ?? 0)/(decimal?)x.RatedLife)

            }).ToListAsync();

            int count = await q.CountAsync();

            foreach (var item in data)
            {
                if (item.RatedLife != 0) // 假设RatedLife不应为0以避免除以零错误  
                {
                    item.Percent = Math.Round((decimal)((item.RunLife * 100) / item.RatedLife), 1);
                    item.RunLife = Math.Round((decimal)item.RunLife, 1);
                }
            }

            // 增加不分页判断
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
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 获取信号量的值
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="trainNum"></param>
        /// <param name="coachName"></param>
        /// <param name="code"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isTop1"></param>
        /// <returns></returns>
        [HttpGet("SignalVal")]
        public AjaxResult<object> GetSignalVal(int? trainId = default, string? trainNum = default, string? coachName = default, string? code = default, DateTime? startTime = default, DateTime? endTime = default, bool isTop1 = false)
        {
            var result = new AjaxResult<object>();
            result.Success = true;
            result.Code = 200;

            if (trainId == default && trainNum == default)
            {
                result.Message = "trainId 和 trainNum 不能都为空";
                return result;
            }

            var trainq = sqlSugarClient.Queryable<Train>().Where(x => !x.IsDeleted);

            if (trainId == default)
            {
                var train = trainq.Where(x => x.Name == trainNum).ToList().FirstOrDefault();
                if (train != null)
                    trainId = train.Id;
                else
                {
                    result.Message = "列车不存在";
                    return result;
                }
            }

            var st = DateTime.Now.Date;
            var et = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            if (startTime != default && endTime != default)
            {
                if (startTime.Value.Date != endTime.Value.Date)
                {
                    result.Success = false;
                    result.Message = "只能获取同一天的数据";
                    return result;
                }

                st = startTime.Value;
                et = endTime.Value;
            }

            string wheresql = "";
            wheresql += " and createTime >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            wheresql += " and createTime < '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            var top = " top 1 ";
            var select = $"select lcid, createTime ";
            var selectall = $"select * ";

            if (isTop1)
            {
                select = $"select {top} lcid, createTime ";
                selectall = $"select {top} * ";
            }

            if (code != default)
            {
                select = select + "," + code;
            }

            string sql = $"{select} " +
                        " from TB_PARSING_DATAS" + $"_{st:yyyyMMdd} " +
                        " where lcid='{0}'{1}" +
                        " order by createTime desc";

            if (string.IsNullOrEmpty(code))
            {
                sql = selectall +
                       " from TB_PARSING_DATAS" + $"_{st:yyyyMMdd} " +
                       " where lcid='{0}'{1}" +
                       " order by createTime desc";
            }

            sql = string.Format(sql, trainId, wheresql);

            try
            {
                //var data = sqlSugarClient.SqlQueryable<TB_PARSING_DATAS>(sql).ToList();
                var data = sqlSugarClient.Ado.SqlQuery<dynamic>(sql).ToList();

                result.Success = true;
                result.Data = data;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Data = new List<TB_PARSING_DATAS>();
                result.Message = e.Message;
                return result;
            }


            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new CapitalizeContractResolver(),
            //    Formatting = Formatting.Indented
            //};

            //var json = JsonConvert.SerializeObject(data, settings);


        }

        /// <summary>
        /// 关键数据导出
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet("excel")]
        public IActionResult Export(string? trainId = default, DateTime? startTime = default, DateTime? endTime = default)
        {
            try
            {
                var st = DateTime.Now.Date;
                var et = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                if (startTime != default && endTime != default)
                {
                    if (startTime.Value.Date != endTime.Value.Date)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "只能获取同一天的数据");
                    }

                    st = startTime.Value;
                    et = endTime.Value;
                }

                string wheresql = "";
                wheresql += " and createTime >= '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                wheresql += " and createTime < '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                var selectall = $"select * ";
                var sql = selectall +
                         " from TB_PARSING_DATAS" + $"_{st:yyyyMMdd} " +
                         " where lcid='{0}'{1}" +
                         " order by createTime desc";

                sql = string.Format(sql, trainId, wheresql); 

                string rptTitle = "关键数据导出";
                ExcelUtil.Instance().FileName = $"{rptTitle}.xlsx";
                ExcelUtil.Instance().AliasDataSource.Clear();

                var dataTable = sqlSugarClient.Ado.GetDataTable(sql);
                dataTable.TableName = "Data";
                // 调用 Save 方法获取 MemoryStream  
                using (var stream = ExcelUtil.Instance().Save(dataTable, "关键数据导出模板"))
                {

                    byte[] excelBytes = new byte[stream.Length];
                    stream.Read(excelBytes, 0, excelBytes.Length);

                    // 设置文件名  
                    string fileName = rptTitle + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                    // 返回文件给客户端  
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
            catch (Exception ex)
            {
                // 返回一个错误响应  
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the Excel file.");
            }
        }
    }

    public class TrainStateInfo
    {
        public string trainNum { get; set; }
        public int state { get; set; }
        public int alarm { get; set; }
        public int warning { get; set; }
    }

    public class CapitalizeContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return propertyName;

            // 将属性名称首字母大写
            return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
        }
    }
}

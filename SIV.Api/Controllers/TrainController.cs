using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIV.Api.Authorization.Models;
using SIV.Api.DTO;
using SqlSugar;
using System.Diagnostics;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;

        public TrainController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 获取列车状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("State")]
        public IActionResult GetState()
        {
            var result = new AjaxResult<object>();
#if DEBUG
            string json = "[{\"trainNum\":\"11001002\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11003004\",\"state\":1,\"alarm\":0,\"warning\":1},{\"trainNum\":\"11005006\",\"state\":-1,\"alarm\":8,\"warning\":0},{\"trainNum\":\"11007008\",\"state\":0,\"alarm\":5,\"warning\":3},{\"trainNum\":\"11009010\",\"state\":0,\"alarm\":7,\"warning\":5},{\"trainNum\":\"11011012\",\"state\":0,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11013014\",\"state\":1,\"alarm\":4,\"warning\":1},{\"trainNum\":\"11015016\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11017018\",\"state\":-1,\"alarm\":3,\"warning\":1},{\"trainNum\":\"11019020\",\"state\":-1,\"alarm\":0,\"warning\":8},{\"trainNum\":\"11021022\",\"state\":0,\"alarm\":4,\"warning\":3},{\"trainNum\":\"11023024\",\"state\":-1,\"alarm\":9,\"warning\":4},{\"trainNum\":\"11025026\",\"state\":1,\"alarm\":6,\"warning\":0},{\"trainNum\":\"11027028\",\"state\":1,\"alarm\":9,\"warning\":9},{\"trainNum\":\"11029030\",\"state\":0,\"alarm\":9,\"warning\":3},{\"trainNum\":\"11031032\",\"state\":-1,\"alarm\":4,\"warning\":3},{\"trainNum\":\"11033034\",\"state\":1,\"alarm\":6,\"warning\":6},{\"trainNum\":\"11035036\",\"state\":1,\"alarm\":5,\"warning\":5},{\"trainNum\":\"11037038\",\"state\":0,\"alarm\":0,\"warning\":1},{\"trainNum\":\"11039040\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11041042\",\"state\":1,\"alarm\":0,\"warning\":8},{\"trainNum\":\"11043044\",\"state\":1,\"alarm\":4,\"warning\":6},{\"trainNum\":\"11045046\",\"state\":-1,\"alarm\":2,\"warning\":0},{\"trainNum\":\"11047048\",\"state\":-1,\"alarm\":2,\"warning\":4},{\"trainNum\":\"11049050\",\"state\":1,\"alarm\":9,\"warning\":8},{\"trainNum\":\"11051052\",\"state\":1,\"alarm\":1,\"warning\":2},{\"trainNum\":\"11053054\",\"state\":1,\"alarm\":4,\"warning\":8},{\"trainNum\":\"11055056\",\"state\":0,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11057058\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11059060\",\"state\":-1,\"alarm\":1,\"warning\":5},{\"trainNum\":\"11061062\",\"state\":0,\"alarm\":1,\"warning\":0},{\"trainNum\":\"11063064\",\"state\":0,\"alarm\":9,\"warning\":6},{\"trainNum\":\"11065066\",\"state\":1,\"alarm\":4,\"warning\":9},{\"trainNum\":\"11067068\",\"state\":0,\"alarm\":9,\"warning\":2},{\"trainNum\":\"11069070\",\"state\":1,\"alarm\":8,\"warning\":8},{\"trainNum\":\"11071072\",\"state\":0,\"alarm\":0,\"warning\":2},{\"trainNum\":\"11073074\",\"state\":1,\"alarm\":0,\"warning\":5},{\"trainNum\":\"11075076\",\"state\":0,\"alarm\":6,\"warning\":3},{\"trainNum\":\"11077078\",\"state\":-1,\"alarm\":2,\"warning\":4},{\"trainNum\":\"11079080\",\"state\":1,\"alarm\":8,\"warning\":4},{\"trainNum\":\"11081082\",\"state\":0,\"alarm\":5,\"warning\":9},{\"trainNum\":\"11083084\",\"state\":-1,\"alarm\":6,\"warning\":1},{\"trainNum\":\"11085086\",\"state\":-1,\"alarm\":6,\"warning\":0},{\"trainNum\":\"11087088\",\"state\":1,\"alarm\":5,\"warning\":3},{\"trainNum\":\"11089090\",\"state\":0,\"alarm\":4,\"warning\":0},{\"trainNum\":\"11091092\",\"state\":-1,\"alarm\":7,\"warning\":7},{\"trainNum\":\"11093094\",\"state\":0,\"alarm\":1,\"warning\":0},{\"trainNum\":\"11095096\",\"state\":-1,\"alarm\":2,\"warning\":0},{\"trainNum\":\"11097098\",\"state\":-1,\"alarm\":4,\"warning\":8},{\"trainNum\":\"11099100\",\"state\":0,\"alarm\":7,\"warning\":6}]";
            var data = JsonConvert.DeserializeObject(json);

            result.Success = true;
            result.Data = data;
            return Ok(result);

#endif        

            result.Success = true;
            return Ok(result);
        }

        [HttpGet]
        public AjaxResult<List<Train>> Get()
        {
            var result = new AjaxResult<List<Train>>();
            result.Code = 200;

            result.Data = sqlSugarClient.Queryable<Train>().ToList();

            return result;
        }

        /// <summary>
        /// 列车添加
        /// </summary>
        /// <param name="train"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Add(Train train)
        {
            var result = new AjaxResult<string>();
            result.Success = false;
            result.Code = 200;

            train.CreatedTime = DateTime.Now;
            train.UpdatedTime = DateTime.Now;

            var count = sqlSugarClient.Insertable(train).ExecuteCommand();

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
        public AjaxResult<string> Update(Train train)
        {
            var result = new AjaxResult<string>();
            result.Success = false;
            result.Code = 200;
             
            train.UpdatedTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(train).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                return result;
            }

            return result;
        }
    }
}

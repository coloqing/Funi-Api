using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SqlSugar;
using System.Collections.Generic;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly ISqlSugarClient db;

        public LinesController(SqlSugarClient db)
        {
            this.db = db;
        }

        /// <summary>
        /// 获取线路
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult<List<LineDTO>> GetLines()
        {
            var list = db.Queryable<LineDTO>().Where(x => !x.IsDeleted).ToList();
            var result = new AjaxResult<List<LineDTO>>();
            result.Data = list;
            result.Success = true;

            return result;
        }

        /// <summary>
        /// 添加线路
        /// </summary>
        /// <param name="lineDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(LineDTO lineDTO)
        {
            lineDTO.CreatedTime = DateTime.Now;
            lineDTO.UpdatedTime = DateTime.Now;
            var count = db.Insertable(lineDTO).ExecuteCommand();

            var result = new AjaxResult<string>();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "新增成功";
            }
            else
            {
                result.Success = false;
                result.Message = "新增失败";
            }

            return Ok(result);
        }

        /// <summary>
        /// 更新线路信息
        /// </summary>
        /// <param name="lineDTO"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public IActionResult Update(LineDTO lineDTO)
        {
            lineDTO.UpdatedTime = DateTime.Now;

            var count = db.Updateable(lineDTO).ExecuteCommand();

            var result = new AjaxResult<string>();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "修改成功";
            }
            else
            {
                result.Success = false;
                result.Message = "修改失败";
            }

            return Ok(result);
        }
    }
}

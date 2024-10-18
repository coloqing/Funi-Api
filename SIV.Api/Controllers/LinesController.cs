using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SqlSugar;
using System.Collections.Generic;
using System.Linq.Expressions;
using Util.DTO;
using SIV.Util;
using SIV.Api.Models;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly ISqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public LinesController(SqlSugarClient db)
        {
            this.sqlSugarClient = db;
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LineDTO, Line>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
           );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 获取线路
        /// </summary>
        /// <param name="lineId">线路Id</param>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<LineDTO>> GetLines(string? lineId = default, string? province = default, string? city = default, int? pageIndex = 1, int? pageRow = 10) 
        {
            var expression = Expressionable.Create<Line>();
            if (!string.IsNullOrEmpty(lineId))
                expression.And(x => x.LineId == lineId);
            if (!string.IsNullOrEmpty(province))
                expression.And(x => x.Province == province);
            if (!string.IsNullOrEmpty(city))
                expression.And(x => x.City == city);
            expression.And(x => !x.IsDeleted); 

            var query = sqlSugarClient.Queryable<Line>().Where(expression.ToExpression());
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<LineDTO>
            {
                Data = mapper.Map<List<LineDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加线路
        /// </summary>
        /// <param name="lineDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(LineDTO lineDTO)
        {
            var line = mapper.Map<Line>(lineDTO);

            line.LineId = lineDTO.Name;
            line.IsDeleted = false;
            line.CreateTime = DateTime.Now;
            line.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Insertable(line).ExecuteCommand();

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
            var dbline = sqlSugarClient.Queryable<Line>().First(x=>x.Id == lineDTO.Id);

            mapper.Map(lineDTO, dbline);

            dbline.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbline).ExecuteCommand();

            var result = new AjaxResult<string>();

            if (count > 0)
            {
                result.Code = 200;
                result.Success = true;
                result.Message = "修改成功";
            }
            else
            {
                result.Code = 200;
                result.Success = false;
                result.Message = "修改失败";
            }

            return Ok(result);
        }
 
    }

    public class LineViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LineId { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? Grouping { get; set; }
        public int? DevicePerCarriage { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}

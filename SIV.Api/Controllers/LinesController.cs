using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SqlSugar;
using System.Collections.Generic;
using System.Linq.Expressions;
using Util.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly ISqlSugarClient db;
        private IMapper mapper;

        public LinesController(SqlSugarClient db)
        {
            this.db = db;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LineDTO, LineDTO>()
            .ForMember(x=>x.CreatedTime,op=>op.Ignore())
            .ForMember(x=>x.UpdatedTime,op=>op.Ignore())
            .ForMember(x=>x.IsDeleted,op=>op.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null))
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
        public AjaxResult<List<LineDTO>> GetLines(string? lineId = default, string? province = default, string? city = default)
        {
            var expression = Expressionable.Create<LineDTO>();
            if (!string.IsNullOrEmpty(lineId))
                expression.And(x => x.LineId == lineId);
            if (!string.IsNullOrEmpty(province))
                expression.And(x => x.Province == province);
            if (!string.IsNullOrEmpty(city))
                expression.And(x => x.City == city);
            expression.And(x => !x.IsDeleted);

            var list = db.Queryable<LineDTO>().Where(expression.ToExpression()).ToList(); 

            var result = new AjaxResult<List<LineDTO>>();
            result.Code = 200;
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
            lineDTO.LineId = lineDTO.Name;
            lineDTO.IsDeleted = false;
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
            var dbline = db.Queryable<LineDTO>().First(x=>x.Id == lineDTO.Id);

            mapper.Map(lineDTO, dbline);

            dbline.UpdatedTime = DateTime.Now;

            var count = db.Updateable(dbline).ExecuteCommand();

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

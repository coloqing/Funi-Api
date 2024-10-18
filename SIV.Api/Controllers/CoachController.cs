using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.Authorization.Models;
using SIV.Api.DTO;
using SqlSugar;
using SIV.Util;
using Util.DTO;
using SIV.Api.Models;
using System.Collections.Generic;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public CoachController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CoachDTO, Coach>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                }
            );
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<PageResult<CoachDTO>> Get(string? lineName = default, string? trainNum = default, int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<Coach>().Where(x => !x.IsDeleted);
            query.WhereIF(!string.IsNullOrEmpty(lineName), x => x.LineName == lineName);
            query.WhereIF(!string.IsNullOrEmpty(trainNum), x => x.TrainNum == trainNum);

            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<CoachDTO>
            {
                Data = mapper.Map<List<CoachDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] CoachDTO coachdto)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var coach = mapper.Map<Coach>(coachdto);

            coach.CreateTime = DateTime.Now;
            coach.UpdateTime = DateTime.Now;
            coach.IsDeleted = false;

            var count = sqlSugarClient.Insertable(coach).ExecuteCommand();

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

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(CoachDTO coach)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcocach = sqlSugarClient.Queryable<Coach>().First(x => x.Id == coach.Id && !x.IsDeleted);

            mapper.Map(coach, dbcocach);

            dbcocach.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbcocach).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
            }
            else
            {
                result.Success = false;
                result.Message = "更新失败";
            }

            return result;
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="coach"></param>
        /// <returns></returns>
        [HttpPost("UpdateMuch")]
        public AjaxResult<string> UpdateMuch(List<CoachDTO> coachs)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            List<Coach> updateList = new List<Coach>();

            foreach (var coach in coachs) {
                var dbcocach = sqlSugarClient.Queryable<Coach>().First(x => x.Id == coach.Id && !x.IsDeleted);

                mapper.Map(coach, dbcocach);

                dbcocach.UpdateTime = DateTime.Now;
                updateList.Add(dbcocach);
            }

            var count = sqlSugarClient.Updateable(updateList).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
            }
            else
            {
                result.Success = false;
                result.Message = "更新失败";
            }

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dcoach"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteCoach dcoach)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcocach = sqlSugarClient.Queryable<Coach>().First(x => x.Id == dcoach.Id && !x.IsDeleted);

            dbcocach.UpdateTime = DateTime.Now;
            dbcocach.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbcocach).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "删除成功";
            }
            else
            {
                result.Success = false;
                result.Message = "删除失败";
            }

            return result;
        }
    }

    public class DeleteCoach
    {
        public int Id { get; set; }
    }
}

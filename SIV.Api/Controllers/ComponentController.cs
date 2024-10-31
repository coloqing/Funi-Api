using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SIV.Api.Models;
using SqlSugar;
using SIV.Util;
using Util.DTO;

namespace SIV.Api.Controllers
{
    /// <summary>
    /// 设备部件
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public ComponentController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ComponentDTO, Component>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
            );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 获取设备部件
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<ComponentDTO>> Get(string? name = default, string? SN = default, int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<Component>().Where(x => !x.IsDeleted);
            query.WhereIF(name != default, x => x.Name == name);
            query.WhereIF(SN != default, x => x.SN == SN);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<ComponentDTO>
            {
                Data = mapper.Map<List<ComponentDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] ComponentDTO component)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcomponent = mapper.Map<Component>(component);

            dbcomponent.CreateTime = DateTime.Now;
            dbcomponent.UpdateTime = DateTime.Now;
            dbcomponent.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbcomponent).ExecuteCommand();

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
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(ComponentDTO component)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcomponent = sqlSugarClient.Queryable<Component>().First(x => x.Id == component.Id && !x.IsDeleted);

            mapper.Map(component, dbcomponent);

            dbcomponent.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbcomponent).ExecuteCommand();

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
        /// <param name="deleteEntityId"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteEntityId deleteEntityId)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbcomponent = sqlSugarClient.Queryable<Component>().First(x => x.Id == deleteEntityId.Id && !x.IsDeleted);

            dbcomponent.UpdateTime = DateTime.Now;
            dbcomponent.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbcomponent).ExecuteCommand();

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
}

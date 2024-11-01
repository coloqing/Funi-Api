using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SIV.Api.Models;
using SqlSugar;
using Util.DTO;

namespace SIV.Api.Controllers
{
    /// <summary>
    /// 组织架构
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrgStructureController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public OrgStructureController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrgStructureDTO, OrgStructure>().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                cfg.CreateMap<OrgStructure, OrgStructureDTO>();
            }
           );
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public AjaxResult<List<OrgStructureVO>> Get()
        {
            var result = new AjaxResult<List<OrgStructureVO>>();
            result.Code = 200;

            var data = sqlSugarClient.Queryable<OrgStructure>().Where(x => !x.IsDeleted).ToList();

            var list = data.Where(x => x.ParentId == 0);

            result.Data = new List<OrgStructureVO>();

            foreach (var item in list)
            {
                var orgStructure = new OrgStructureVO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    Time = item.UpdateTime,
                    Children = new List<OrgStructureVO>()
                };
                FindChildrens(ref orgStructure, data);

                result.Data.Add(orgStructure);
            }
             
            result.Success = true;
            return result;
        }

        [NonAction]
        public void FindChildrens(ref OrgStructureVO orgStructure, List<OrgStructure> list)
        {
            var id = orgStructure.Id;

            foreach (var item in list.Where(x => x.ParentId == id))
            {
                var child = new OrgStructureVO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    Time = item.UpdateTime
                };

                FindChildrens(ref child, list);

                orgStructure.Children ??= new List<OrgStructureVO>();
                orgStructure.Children.Add(child);
            }

        }

        /// <summary>
        /// 添加组织
        /// </summary>
        /// <param name="orgStructuredto"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Add(OrgStructureDTO orgStructuredto)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var orgStructure = mapper.Map<OrgStructure>(orgStructuredto);

            orgStructure.CreateTime = DateTime.Now;
            orgStructure.UpdateTime = DateTime.Now;
            orgStructure.IsDeleted = false;

            var count = sqlSugarClient.Insertable(orgStructure).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "添加成功";
                return result;
            }

            result.Success = false;
            result.Message = "添加失败";
            return result;
        }

        /// <summary>
        /// 更新组织架构信息
        /// </summary>
        /// <param name="orgStructure"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(OrgStructureDTO orgStructure)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbOrgStructure = sqlSugarClient.Queryable<OrgStructure>().First(x => x.Id == orgStructure.Id && !x.IsDeleted);

            mapper.Map(orgStructure, dbOrgStructure);

            dbOrgStructure.UpdateTime = DateTime.Now; 

            var count = sqlSugarClient.Updateable(dbOrgStructure).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
                return result;
            }

            result.Success = false;
            result.Message = "更新失败";
            return result;
        }

    }

    public class OrgStructureVO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime Time { get; set; }
        public List<OrgStructureVO> Children { get; set; }
    }

}

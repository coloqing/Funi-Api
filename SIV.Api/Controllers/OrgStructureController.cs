using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
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

        public OrgStructureController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;
        }

        [HttpGet]
        public AjaxResult<OrgStructureDTO> Get()
        {
            var result = new AjaxResult<OrgStructureDTO>();
            result.Code = 200;

            var data = sqlSugarClient.Queryable<OrgStructure>().Where(x => !x.IsDeleted).ToList();

            var item = data.First(x => x.ParentId == 0);

            var orgStructure = new OrgStructureDTO
            {
                Id = item.Id,
                Name = item.Name,
                Time = item.UpdatedTime,
                Children = new List<OrgStructureDTO>()
            };

            FindChildrens(ref orgStructure, data);

            result.Data = orgStructure;
            result.Success = true;
            return result;
        }

        [NonAction]
        public void FindChildrens(ref OrgStructureDTO orgStructure, List<OrgStructure> list)
        {
            var id = orgStructure.Id;

            foreach (var item in list.Where(x => x.ParentId == id))
            {
                var child = new OrgStructureDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Time = item.UpdatedTime
                };

                FindChildrens(ref child, list);

                orgStructure.Children ??= new List<OrgStructureDTO>();
                orgStructure.Children.Add(child);
            }

        }

        /// <summary>
        /// 添加组织
        /// </summary>
        /// <param name="orgStructure"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Add(OrgStructure orgStructure)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;



            return result;
        }

        /// <summary>
        /// 更新组织架构信息
        /// </summary>
        /// <param name="orgStructure"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(OrgStructure orgStructure)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;



            return result;
        }

    }

    public class OrgStructureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public List<OrgStructureDTO> Children { get; set; }
    }

}

using SqlSugar;

namespace SIV.Api.DTO
{  
    public class OrgStructureDTO
    {
      
        public int Id { get; set; } 
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; } 
    }
}

using SqlSugar;

namespace SIV.Api.DTO
{ 
    [SugarTable("OrgStructure")]
    public class OrgStructure
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; } 
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

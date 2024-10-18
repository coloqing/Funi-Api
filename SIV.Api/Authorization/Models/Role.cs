using SqlSugar;

namespace SIV.Api.Authorization.Models
{
    [SugarTable("Role")]
    public class Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiIds { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

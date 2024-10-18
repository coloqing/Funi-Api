using SqlSugar;

namespace SIV.Api.Authorization.Models
{ 
    [SugarTable("User")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int? OrgId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? Avatar { get; set; }
        public string? Introduction { get; set; }
        public string? Phone { get; set; }
    }
}

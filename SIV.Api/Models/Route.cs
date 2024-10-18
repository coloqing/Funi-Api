using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Route")]
    public class Route
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 路由路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string Component { get; set; }
        public int ParentId { get; set; }
        public int? Order { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Redirect { get; set; }
        public bool AlwaysShow { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

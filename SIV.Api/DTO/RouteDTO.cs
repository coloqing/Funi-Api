using SqlSugar;

namespace SIV.Api.DTO
{ 
    public class RouteDTO
    { 
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public string? Roles { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// 路由路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string? Component { get; set; }
        public int? ParentId { get; set; }
        public int? Order { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Redirect { get; set; }
        public bool? AlwaysShow { get; set; }
    }
}

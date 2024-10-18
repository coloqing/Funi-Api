using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Line")]
    public class Line
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LineId { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? Grouping { get; set; }
        public int? DevicePerCarriage { get; set; }
        [JsonIgnore]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}

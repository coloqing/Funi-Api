using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.DTO
{
    [SugarTable("Lines")]
    public class LineDTO
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
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}

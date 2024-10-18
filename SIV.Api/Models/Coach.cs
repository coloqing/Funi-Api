using SqlSugar;
using System.Text.Json.Serialization;

namespace SIV.Api.Models
{ 
    [SugarTable("Coach")]
    public class Coach
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? LineId { get; set; }
        public string? LineName { get; set; }
        public string? TrainNum { get; set; }
        public int? TrainId { get; set; }
        public int? DevicePerCarriage { get; set; }
        [JsonIgnore]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}

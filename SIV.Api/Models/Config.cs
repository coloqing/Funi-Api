using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Config")]
    public class Config
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LineName { get; set; }
        public string Data { get; set; }
        [JsonIgnore]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}

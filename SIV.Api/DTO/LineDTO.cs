using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.DTO
{
    [SugarTable("Line")]
    public class LineDTO
    { 
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LineId { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? Grouping { get; set; }
        public int? DevicePerCarriage { get; set; } 
    }
}

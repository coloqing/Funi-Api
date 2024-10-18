using SqlSugar;
using System.Text.Json.Serialization;

namespace SIV.Api.DTO
{
    public class CoachDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? LineId { get; set; }
        public string? LineName { get; set; }
        public string? TrainNum { get; set; }
        public int? TrainId { get; set; }
        public int? DevicePerCarriage { get; set; }

    }
}

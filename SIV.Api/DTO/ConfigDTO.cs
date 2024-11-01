using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.DTO
{ 
    public class ConfigDTO
    { 
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LineName { get; set; }
        public string? Data { get; set; }
    }
}

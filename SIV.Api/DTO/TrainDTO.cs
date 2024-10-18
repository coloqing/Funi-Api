using SqlSugar;

namespace SIV.Api.DTO
{  
    public class TrainDTO
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public int LineId { get; set; } 
    }
}

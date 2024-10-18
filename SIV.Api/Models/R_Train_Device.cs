using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("R_Train_Device")]
    public class R_Train_Device
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int TrainId { get; set; }
        public string TrainNum { get; set; }
        public string CoachType { get; set; }
        public int DeviceId { get; set; }
        public string DeviceSN { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
    
}

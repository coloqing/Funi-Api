namespace SIV.Api.DTO
{
    public class R_Train_DeviceDTO
    { 
        public int Id { get; set; }
        public int TrainId { get; set; }
        public string TrainNum { get; set; }
        public string CoachType { get; set; }
        public int DeviceId { get; set; }
        public string DeviceSN { get; set; } 
    }
}

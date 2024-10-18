using SqlSugar;

namespace SIV.Api.DTO
{ 
    public class DeviceDTO
    {  
        public int Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 列车号
        /// </summary>
        public string TrainNum { get; set; }
        /// <summary>
        /// 车厢类型
        /// </summary>
        public string CoachType { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineName { get; set; } 
    }
}

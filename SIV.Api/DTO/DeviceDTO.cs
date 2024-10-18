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
        /// 关联部件id，逗号分割
        /// </summary>
        public string ComponentIds { get; set; }
    }
}

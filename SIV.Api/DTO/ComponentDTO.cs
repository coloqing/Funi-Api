namespace SIV.Api.Models
{ 
    public class ComponentDTO
    { 
        public int Id { get; set; }
        /// <summary>
        /// 部件编号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        ///// <summary>
        ///// 关联设备Id
        ///// </summary>
        //public int DeviceId { get; set; }
        ///// <summary>
        ///// 关联设备编号
        ///// </summary>
        //public string DeviceSN { get; set; }
        /// <summary>
        /// 关联性能指标id,逗号分割
        /// </summary>
        public string IndicatorsIds { get; set; }

        /// <summary>
        /// 信号量代码
        /// </summary>
        public string SignalCode { get; set; }
    }
}

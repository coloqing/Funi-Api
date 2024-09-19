namespace SIV.Api.DTO
{
    public class TrainFaultDTO
    {
        public string Id { get; set; }

        /// <summary>
        /// 线路
        /// </summary>
        public string TrainLine { get; set; }

        /// <summary>
        /// 列车号
        /// </summary>
        public string TrainNumber { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
            
        public string Type { get; set; }

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart { get; set; }

        /// <summary>
        /// 预警状态
        /// </summary>
        public string FaultSta { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 司机解决建议
        /// </summary>
        public string DriverSuggest { get; set; }

        /// <summary>
        /// 回库检修建议
        /// </summary>
        public string RedSuggest { get; set; }
    }
}

namespace SIV.Api.DTO
{
    /// <summary>
    /// 故障预警统计
    /// </summary>
    public class FaultWarnCount
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string? Time { get; set; }

        /// <summary>
        /// 预警数
        /// </summary>
        public int WarnNum { get; set; }

        /// <summary>
        /// 故障数
        /// </summary>
        public int FaultNum { get; set; }

    }

    /// <summary>
    /// 故障预警统计
    /// </summary>
    public class FaultWarnDisPercent
    {
       
        /// <summary>
        /// 已处置率
        /// </summary>
        public float Dispose { get; set; }

        /// <summary>
        /// 未处置lv
        /// </summary>
        public float NoDispose { get; set; }

    }

    /// <summary>
    /// 故障预警统计
    /// </summary>
    public class FaultWarnTop10
    {

        /// <summary>
        /// 列车号
        /// </summary>
        public string? TrainNumber { get; set; }

        /// <summary>
        /// 故障数
        /// </summary>
        public int Count { get; set; }

    }
}

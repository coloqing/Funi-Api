namespace GZSAC.Api.DTO
{
    public class FaultWarnNum
    {
        /// <summary>
        /// 健康期空调数量
        /// </summary>
        public int HealthNum {  get; set; }

        /// <summary>
        /// 亚健康期空调数量
        /// </summary>
        public int SubHealthNum {  get; set; }

        /// <summary>
        /// 故障期空调数量
        /// </summary>
        public int UnHealthNum {  get; set; }

        /// <summary>
        /// 预警空调总数
        /// </summary>
        public int WarnSum {  get; set; }

        /// <summary>
        /// 故障空调总数
        /// </summary>
        public int FaultSum {  get; set; }

    }
}

namespace SIV.Api.DTO
{
    /// <summary>
    /// 列车状态数量
    /// </summary>
    public class TrainNumDTO
    {
        /// <summary>
        /// 总列车数
        /// </summary>
        public int AllNum { get; set; }

        /// <summary>
        /// 在线列车数
        /// </summary>
        public int OnlineNum { get; set; }

        /// <summary>
        /// 在线正常列车数量
        /// </summary>
        public int NomalNum {  get; set; }

        /// <summary>
        /// 在线预警列车数量
        /// </summary>
        public int WarnNum {  get; set; }

        /// <summary>
        /// 在线故障列车数量
        /// </summary>
        public int FaultNum {  get; set; }
    }
}

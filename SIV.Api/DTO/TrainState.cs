namespace SIV.Api.DTO
{
    public class TrainState
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id {  get; set; }

        /// <summary>
        /// 列车号
        /// </summary>
        public string? TrainNumber {  get; set; }

        /// <summary>
        /// 列车状态 0：离线 1：在线 2：库内
        /// </summary>
        public int State { get; set; } = 0;

        /// <summary>
        /// 预警数
        /// </summary>
        public int WarnNum {  get; set; }   

        /// <summary>
        /// 故障数
        /// </summary>
        public int FaultNum {  get; set; }
    }
}

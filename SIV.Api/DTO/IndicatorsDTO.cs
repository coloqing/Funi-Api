namespace SIV.Api.Models
{
    public class IndicatorsDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 最大阈值
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// 最小阈值
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// 关联信号量代码
        /// </summary>
        public string SignalCode { get; set; }

        public object? Value { get; set; }
        public int State { get; set; }
    }
}

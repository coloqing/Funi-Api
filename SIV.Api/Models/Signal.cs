using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Signal")]
    public class Signal
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 信号量名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应卡夫卡数据字段
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 信号类型：Analog = 模拟量 Digital = 数字量
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string? Units { get; set; }
        [JsonIgnore]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
    }
}

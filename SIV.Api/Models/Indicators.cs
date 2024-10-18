using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Indicators")]
    public class Indicators
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
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
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

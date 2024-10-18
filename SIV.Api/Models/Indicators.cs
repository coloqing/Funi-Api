using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Indicators")]
    public class Indicators
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// �����ֵ
        /// </summary>
        public float Max { get; set; }
        /// <summary>
        /// ��С��ֵ
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// �����ź�������
        /// </summary>
        public string SignalCode { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

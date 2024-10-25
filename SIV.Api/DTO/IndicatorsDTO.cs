namespace SIV.Api.Models
{
    public class IndicatorsDTO
    {
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

        public object? Value { get; set; }
        public int State { get; set; }
    }
}

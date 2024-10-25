using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("Component")]
    public class Component
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// �����豸Id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// �����豸���
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// ��������ָ��id,���ŷָ�
        /// </summary>
        public string IndicatorsIds { get; set; }
        /// <summary>
        /// �ź�������
        /// </summary>
        public string SignalCode { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
    
}

using Newtonsoft.Json;
using SqlSugar;

namespace SIV.Api.Models
{
    [SugarTable("PartsLife")]
    public class PartsLife
    { 
        /// <summary>
        /// ��·
        /// </summary>           

        public string? XL { get; set; }

        /// <summary>
        /// ����
        /// </summary>           

        public string? CH { get; set; }

        /// <summary>
        /// ����
        /// </summary>           

        public string? CX { get; set; }

        /// <summary>
        /// ����λ������
        /// </summary>           
        public string? WZName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>

        public string? Name { get; set; }

        /// <summary>
        /// ����
        /// </summary>           
        public string? TypeName { get; set; }

        /// <summary>
        /// �Ѻ�����/����
        /// </summary>           

        public decimal? RunLife { get; set; }

        /// <summary>
        /// �����/����
        /// </summary>           

        public decimal? RatedLife { get; set; }

        /// <summary>
        /// ʣ������/����
        /// </summary>           

        public decimal? SurplusLife { get; set; }

        /// <summary>
        /// �ٷֱ�
        /// </summary>           

        public decimal? Percent { get; set; }

        /// <summary>
        /// �ٷֱ�
        /// </summary>           

        public string? Suggest { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// ����λ��
        /// </summary>           
        public int? WZ { get; set; }

        /// <summary>
        /// ����
        /// </summary>           

        public string? Type { get; set; }

        public string FaultCode { get; set; }
        public string Code { get; set; }
        public string FarecastCode { get; set; }

        /// <summary>
        /// Desc:����ʱ��
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "����ʱ��")]
        public DateTime? createtime { get; set; }

        /// <summary>
        /// Desc:������
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "������", Length = 50)]
        public string? createuserid { get; set; }

        /// <summary>
        /// Desc:�޸�ʱ��
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "�޸�ʱ��")]
        public DateTime? updatetime { get; set; }

        /// <summary>
        /// Desc:�޸���
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "�޸���", Length = 50)]
        public string? updateuserid { get; set; }
    }
}

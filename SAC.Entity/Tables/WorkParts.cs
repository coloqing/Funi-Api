using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Entity
{
    /// <summary>
    /// 工作部件表
    /// </summary>
    [SugarTable("WorkParts")]
    public class WorkParts :BaseEntity
    {    
        /// <summary>
        /// 部件名称
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件名称")]
        public string? PartsName { get; set; }
        /// <summary>
        /// 部件编码
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件编码")]
        public string? PartsNameCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>           
        [SugarColumn(ColumnDescription = "设备编码", Length = 50)]
        public string? device_code { get; set; }

        /// <summary>
        /// 部件状态 0：正常 1：预警 2：故障
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件状态 0：正常 1：预警 2：故障")]
        public int? PartsState { get; set; }
        /// <summary>
        /// 部件类型 0：预警 1：故障 2：寿命 3：状态
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件类型 0：预警 1：故障 2：寿命 3：状态")]
        public int? PartsType { get; set; }
        /// <summary>
        /// 部件寿命
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件寿命")]
        public int? PartsLife { get; set; }
        /// <summary>
        /// 额定寿命
        /// </summary>           
        [SugarColumn(ColumnDescription = "额定寿命")]
        public int? FixedLife { get; set; }
        /// <summary>
        /// 剩余寿命
        /// </summary>           
        [SugarColumn(ColumnDescription = "剩余寿命")]
        public int? SurplusLife { get; set; }
        /// <summary>
        /// 部件模式 0：无效 1：有效
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件模式 0：无效 1：有效")]
        public int? PartsModle{ get; set; }

    }
}

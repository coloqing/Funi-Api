using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Entity
{
    [SugarTable("FaultOrWarn")]
    public class FaultOrWarn
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键ID  故障预警表")]
        public int Id { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        [SugarColumn(ColumnDescription = "线路名称")]
        public string xlh { get; set; }

        [SugarColumn(ColumnDescription = "列车号")]
        public string lch { get; set; }
        public string cxh { get; set; }
        public string DeviceCode { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string createtime { get; set; }
        public string updatetime { get; set; }

    }
}

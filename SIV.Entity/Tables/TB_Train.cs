using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entity.Tables
{
    /// <summary>
    /// 列车信息表
    /// </summary>
    [SugarTable("TB_Train")]
    public class TB_Train : BaseEntity
    {
        /// <summary>
        /// 列车号
        /// </summary>
        public string? TrainNumber { get; set; }

        /// <summary>
        /// 列车线路
        /// </summary>
        public string? TrainLine { get; set; }

        /// <summary>
        /// 列车状态 0：离线 1：在线 2：库内
        /// </summary>
        public int State { get; set; } = 0;
     
    }
}

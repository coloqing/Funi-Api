using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entity
{
    /// <summary>
    /// 部件寿命表
    /// </summary>
    [SugarTable("PartsLife")]
    public class PartsLife :BaseEntity
    {
        /// <summary>
        /// 部件名称
        /// </summary>
        [SugarColumn(ColumnDescription = "部件名称", Length = 255)]
        public string? PartsName { get; set; }

        /// <summary>
        /// 线路
        /// </summary>           
        [SugarColumn(ColumnDescription = "线路", Length = 255)]
        public string? LineName { get; set; }

        /// <summary>
        /// 车号
        /// </summary>           
        [SugarColumn(ColumnDescription = "车号", Length = 255)]
        public string? TrainNumber { get; set; }

        /// <summary>
        /// 车厢
        /// </summary>           
        [SugarColumn(ColumnDescription = "车厢", Length = 255)]
        public string? CarriageNuber { get; set; }

        /// <summary>
        /// 部件位置
        /// </summary>           
        [SugarColumn(ColumnDescription = "部件位置", Length = 255)]
        public int? PartsPosition { get; set; }

        /// <summary>
        /// 类型
        /// </summary>           
        [SugarColumn(ColumnDescription = "类型", Length = 255)]
        public string? Type { get; set; }

        /// <summary>
        /// 已耗寿命/次数
        /// </summary>           
        [SugarColumn(ColumnDescription = "已耗寿命/次数")]
        public decimal? RunLife { get; set; }

        /// <summary>
        /// 额定寿命/次数
        /// </summary>           
        [SugarColumn(ColumnDescription = "额定寿命/次数")]
        public decimal? RatedLife { get; set; }

        /// <summary>
        /// 剩余寿命/次数
        /// </summary>           
        [SugarColumn(ColumnDescription = "剩余寿命/次数")]
        public decimal? SurplusLife { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>           
        [SugarColumn(ColumnDescription = "百分比")]
        public decimal? Percent { get; set; }
    }
}

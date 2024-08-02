using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAC.Entity
{
    /// <summary>
    /// 系统字典表
    /// </summary>
    [SugarTable("TB_SYS_DIC")]
    public class TB_SYS_DIC
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public string Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnDescription = "编码")]
        [MaxLength(100)]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(ColumnDescription = "值")]
        [MaxLength(100)]
        public string? Value { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        [SugarColumn(ColumnDescription = "父编码")]
        [MaxLength(50)]
        public string ParentId { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        [SugarColumn(ColumnDescription = "顺序号")]
        public Decimal? Sort { get; set; }
    }
    public class BaseDicEntity
    {
        /// <summary>
        /// 返回的标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 返回的值
        /// </summary>

        public string Value { get; set; }

    }
}
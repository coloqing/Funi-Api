using SAC.Entity.DTO;
using SAC.Util;
using SqlSugar;

namespace SAC.Entity
{
    ///<summary>
    ///实时数据
    ///</summary>
    [Map(typeof(TB_PARSING_DATAS))]
    [SugarTable("TB_PARSING_NEWDATAS")]
    public partial class TB_PARSING_NEWDATAS : TB_PARSING_DATAS_Base
    {
        /// <summary>
        /// Desc:更新时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "更新时间")]
        public DateTime update_time { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>           
        [SugarColumn(ColumnDescription = "设备状态")]
        public int State { get; set; }

    }
}

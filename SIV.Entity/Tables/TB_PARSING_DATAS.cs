using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace SIV.Entity
{
    ///<summary>
    ///解析转换后数据
    ///</summary>

    [SplitTable(SplitType.Day)]
    [SugarTable("TB_PARSING_DATAS_{year}{month}{day}")]
    public partial class TB_PARSING_DATAS : TB_PARSING_DATAS_Base
    {
      
    }
}

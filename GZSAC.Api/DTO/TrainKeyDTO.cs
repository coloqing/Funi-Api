using SAC.Entity;
using SAC.Util;
using SqlSugar;

namespace GZSAC.Api.DTO
{
    public class TrainKeyDTO : TB_PARSING_NEWDATAS
    {
        /// <summary>
        /// 通风模式名称
        /// </summary>
        public string tfmsName { get; set; }
    }
}

using SIV.Entity;
using SIV.Util;
using SqlSugar;

namespace SIV.Api.DTO
{
    public class TrainKeyDTO : TB_PARSING_NEWDATAS
    {
        /// <summary>
        /// 通风模式名称
        /// </summary>
        public string tfmsName { get; set; }
    }
}

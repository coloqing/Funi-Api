using SAC.Entity;
using SAC.Util;
using SqlSugar;

namespace SAC.Entity.DTO
{
    [Map(typeof(TB_PARSING_NEWDATAS))]
    public class TrainKeyDTO : TB_PARSING_NEWDATAS
    {
        /// <summary>
        /// 通风模式名称
        /// </summary>
        public string tfmsName {  get; set; }
    }
}

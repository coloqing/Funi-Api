namespace GZSAC.Api.DTO
{
    public class LCH
    {
        public string lch {  get; set; }

        public string cxh {  get; set; }

        /// <summary>
        /// 列车状态 0：离线 1：在线 2：库内
        /// </summary>
        public int State { get; set; } = 0;
    }
}

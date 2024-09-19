namespace SIV.Api.DTO.OldDTO
{
    public class JsonMain
    {
        private string msg = string.Empty;
        private JsonStatus status = JsonStatus.error;

        /// <summary>
        /// 状态
        /// </summary>
        public JsonStatus Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg
        {
            get { return this.msg; }
            set
            {
                this.msg = value;
            }
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; }

    }

    public enum JsonStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        success = 200,
        /// <summary>
        /// 异常状态
        /// </summary>
        error = 500
    }
}

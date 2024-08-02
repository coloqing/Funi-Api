namespace GZSAC.Api.DTO
{
    public class TrainStatu
    {
        /// <summary>
        /// 线路编码
        /// </summary>
        public string line_code { get; set; }
        /// <summary>
        /// 总里程
        /// </summary>
        public int miles { get; set; }
        /// <summary>
        /// 总车辆数
        /// </summary>
        public int total_train_num { get; set; }
        /// <summary>
        /// 在线车辆数
        /// </summary>
        public int online_train_num { get; set; }

        /// <summary>
        /// 库内车辆数
        /// </summary>
        public int depot_train_num { get; set; }

        /// <summary>
        /// 离线车辆数
        /// </summary>
        public int offline_train_num { get; set; }
        /// <summary>
        /// 故障车辆数
        /// </summary>
        public int fault_train_num { get; set; }

        /// <summary>
        /// 故障数
        /// </summary>
        public int fault_num { get; set; }
        /// <summary>
        /// 在线车辆
        /// </summary>
        public string[]? online_trains { get; set; }
        /// <summary>
        /// 库内车辆数
        /// </summary>
        public string[]? depot_trains { get; set; }
        /// <summary>
        /// 离线车辆列表
        /// </summary>
        public string[]? offline_trains { get; set; }
        /// <summary>
        /// 故障车辆
        /// </summary>
        public string[]? fault_trains { get; set; }
    }

    public class HttpTrainStaDTO
    {
        public bool success { get; set; }
        public string result_code { get; set; }
        public string result_msg { get; set; }
        public List<TrainStatu> result_data { get; set; }
    }   
}

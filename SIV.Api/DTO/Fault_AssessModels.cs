namespace SIV.Api.DTO
{
    /// <summary>
    /// 寿命推送请求数据
    /// </summary>
    public class Fault_AssessModels
    {
        public string app_id { get; set; }
        public string app_token { get; set; }
        public List<New_FaultsModels> new_faults { get; set; }
        public List<End_FaultsModels> end_faults { get; set; }
    }

    public class New_FaultsModels
    {
        public string line_code { get; set; }
        public string train_code { get; set; }
        public string coach_no { get; set; }
        public string fault_code { get; set; }
        public string fault_url { get; set; }
        public string access_time { get; set; }
    }

    public class End_FaultsModels
    {
        public string line_code { get; set; }
        public string train_code { get; set; }
        public string coach_no { get; set; }
        public string fault_code { get; set; }
        public string access_time { get; set; }
    }
}

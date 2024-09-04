namespace GZSAC.Api.DTO
{
	/// <summary>
	/// 平台故障
	/// </summary>
    public class Records
    {
		public string line_code {  get; set; }
		public string train_code {  get; set; }
		public string coach_no {  get; set; }
		public string? system_name {  get; set; }
		public string? fault_code {  get; set; }
		public string? fault_name {  get; set; }
		public string? fault_desc {  get; set; }
		public string? fault_level {  get; set; }
		public string? happen_time {  get; set; }
		public string? dispose_advice {  get; set; }
		public string? fault_part {  get; set; }
    }

    public class HttpWarnResp
	{
        public List<Records> records { get; set; }
		public int total { get; set; }		
    }
}

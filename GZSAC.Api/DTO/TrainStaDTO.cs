namespace GZSAC.Api.DTO
{
    public class TrainStaDTO
    {
        public string lch { get; set; }
        public string cxh { get; set; }
        public string device_code { get; set; }
        public string yxtzjid { get; set; }
        public string State { get; set; }
        public string FaultState { get; set; }
        public DateTime createtime { get; set; }
    }



    public class LchGroup
    {
        public string Lch { get; set; }

        public List<CxhGroup> CxhGroups { get; set; }

        public LchGroup(string lch)
        {
            Lch = lch;
            CxhGroups = new List<CxhGroup>();
        }
    }

    public class CxhGroup
    {
        public string Cxh { get; set; }
        public List<JZ> YxtzjidList { get; set; }

        public CxhGroup(string cxh)
        {
            Cxh = cxh;
            YxtzjidList = new List<JZ>();
        }
    }

    public class JZ
    {
        public string Jz { get; set; }
        public string JzName { get; set; }
    }
}

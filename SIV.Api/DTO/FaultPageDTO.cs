using SqlSugar;

namespace SIV.Api.DTO
{
    public class FaultPageDTO
    {
        public int Xh { get; set; }
        public string Id { get; set; }  
        public string xlh { get; set; }
        
        public string lch { get; set; }

        public string cxh { get; set; }    
      
        public string TypeName { get; set; }
            
        public string HvacTypeName { get; set; }

        public string FaultName { get; set; }
        public string StateName { get; set; }
        public string createtime { get; set; }
        public string updatetime { get; set; }
        public string DriverSln { get; set; }

        public string Solution { get; set; }

        public string device_code { get; set; }

        public string FaultCode { get; set; }

        public int HvacType { get; set; }

        public string Type { get; set; }
        public string State { get; set; }
    }
}

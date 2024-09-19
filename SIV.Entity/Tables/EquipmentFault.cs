using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entity
{
    [SugarTable("EquipmentFault")]
    public class EquipmentFault
    {
        public int Id { get; set; }
        public string FaultCode { get; set; }
        public int HvacType { get; set; }
        public string FaultName { get; set; }
        public string AnotherName { get; set; }
        public string FaultLevel { get; set; }
        public string FaultRelevant { get; set; }
        public string MvbName { get; set; }
        public string MvbKey { get; set; }
        public string RelevantDeviceCode { get; set; }
        public int IsAffectRun { get; set; }
        public string CheckParts { get; set; }
        public string FaultDescribe { get; set; }
        public string Solution { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public string CXH { get; set; }
        public string DriverSln { get; set; }

    }
}

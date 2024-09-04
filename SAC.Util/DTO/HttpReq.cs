using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Util.DTO
{
    public class HttpReq
    {
        public bool success { get; set; }
        public string result_code { get; set; }
        public string result_msg { get; set; }

    }
}

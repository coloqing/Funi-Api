using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Util.DTO
{
    public class HttpReq<T> : HttpReq
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T? result_data { get; set; }
    }

    public class HttpReqPage<T> : HttpReq
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T? result_data { get; set; }

        /// <summary>
        /// 数据条数
        /// </summary>
        public int? total { get; set; }

        public int? pages { get; set; }    
    }
}

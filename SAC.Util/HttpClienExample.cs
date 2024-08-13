using Newtonsoft.Json;
using SAC.Util.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAC.Util
{
    public class HttpClientExample
    {

        // 假设我们有一个用于处理JSON序列化和反序列化的帮助类
        // 使用Newtonsoft.Json (需要NuGet包 Newtonsoft.Json)
        public static T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        // 发送GET请求并处理返回值的方法
        public static async Task<T?> SendGetRequestAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    //测试数据
                    //string json = "{\r\n    \"success\": true,\r\n    \"result_msg\": \"操作成功！\",\r\n    \"result_code\": \"200\",\r\n    \"result_data\": [\r\n        {\r\n            \"line_code\": \"GZML7S\",\r\n            \"miles\": 0,\r\n            \"total_train_num\": 19,\r\n            \"online_train_num\": 2,\r\n            \"depot_train_num\": 4,\r\n            \"offline_train_num\": 5,\r\n            \"fault_train_num\": 11,\r\n            \"fault_num\": 130,\r\n            \"online_trains\": [\r\n                \"07073074\",\r\n                \"07083084\"\r\n            ],\r\n            \"depot_trains\": [\r\n                \"07077078\",\r\n                \"07081082\",\r\n                \"07087088\",\r\n                \"07089090\"\r\n            ],\r\n            \"offline_trains\": [\r\n                \"07075076\",\r\n                \"07079080\",\r\n                \"07085086\",\r\n                \"07091092\",\r\n                \"07093094\",\r\n                \"07095096\",\r\n                \"07097098\",\r\n                \"07099100\",\r\n                \"07101102\",\r\n                \"07103104\",\r\n                \"07105106\",\r\n                \"07107108\",\r\n                \"07109110\"\r\n            ],\r\n            \"fault_trains\": [\r\n                \"07073074\",\r\n                \"07077078\",\r\n                \"07079080\",\r\n                \"07081082\",\r\n                \"07083084\",\r\n                \"07085086\",\r\n                \"07087088\",\r\n                \"07089090\",\r\n                \"07091092\",\r\n                \"07095096\",\r\n                \"07101102\"\r\n            ]\r\n        }\r\n    ]\r\n}";
                    //return DeserializeJson<T>(json);

                    // 发送GET请求
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return DeserializeJson<T>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    // 处理异常，例如记录错误、返回默认值等
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    return default; // 或者返回null，或者抛出异常，取决于你的需求
                }
            }
        }

        /// <summary>
        /// // 发送POST请求并处理返回值的方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datajson"></param>
        /// <returns></returns>
        public static string HttpClientPost(string url, object datajson)
        {
            HttpClient httpClient = new HttpClient();//http对象
                                                     //表头参数
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //转为链接需要的格式
            HttpContent httpContent = new JsonContent(datajson);
            //请求
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                if (t != null)
                {
                    return t.Result;
                }
            }
            return "";
        }

        public class JsonContent : StringContent
        {
            public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }

    }
}

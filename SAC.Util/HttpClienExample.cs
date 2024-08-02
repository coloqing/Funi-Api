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
                    string json = "{ \"success\":true,\"result_msg\":\"操作成功！\",\"result_code\":\"200\",\"result_data\":[{ \"line_code\":\"GZML7S\",\"miles\":0,\"total_train_num\":19,\"online_train_num\":0,\"depot_train_num\":0,\"offline_train_num\":11,\"fault_train_num\":11,\"fault_num\":118,\"online_trains\":[\"07073074\"],\"depot_trains\":[],\"offline_trains\":[\"07073074\",\"07075076\",\"07077078\",\"07079080\",\"07081082\",\"07083084\",\"07085086\",\"07087088\",\"07089090\",\"07091092\",\"07093094\",\"07095096\",\"07097098\",\"07099100\",\"07101102\",\"07103104\",\"07105106\",\"07107108\",\"07109110\"],\"fault_trains\":[\"07073074\",\"07077078\",\"07079080\",\"07081082\",\"07083084\",\"07085086\",\"07087088\",\"07089090\",\"07091092\",\"07095096\",\"07101102\"]}]}";

                    return DeserializeJson<T>(json);
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

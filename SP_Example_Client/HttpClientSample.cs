using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SP_Example_Client
{
    class HttpClientSample
    {
        static void Main(string[] args)
        {
            //1.Client에서 Server에 “http://127.0.0.1:8088/requestDate”로 요청하고,
            //Server는 현재 날짜와 시각을 Client로 응답하게 하시오. (요청 Method는 ‘GET’ 사용)
            void Run_Http()
            {
                HttpClient client = new HttpClient();
                //GET 방식으로 request함
                var res = client.GetAsync("http://127.0.0.1:8080/requestDate").Result;
                Console.WriteLine("Response :" + " - " + res.Content.ReadAsStringAsync().Result);
                Console.ReadKey();
            }
            //Run_Http();

            //2.Client에서 Server에 접속하여 파일 목록을 json형태로 전송하는 프로그램을 작성하시오.
            //➢ Input 폴더의 파일 목록을 전송하여 Output폴더에 저장
            //- Client는 목록 전송 완료 후 종료
            //- Server는 목록을 수신하여 ‘수신시간.json’파일로 저장하고 다시 Client 접속 대기
            //-요청 Method는 ‘POST’사용
            //- Content Type은 “application / json” 사용
            void Run_Http2()
            {
                DirectoryInfo di = new DirectoryInfo("./Input");
                JObject jsonObj = new JObject();
                jsonObj.Add("Folder", "Input");
                JArray jArray = new JArray();
                foreach (var item in di.GetFiles())
                {
                    jArray.Add(item.Name);
                }
                jsonObj.Add("Files", jArray);

                //request 날리는 방법!
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8080/fileList");
                httpRequest.Content = new StringContent(jsonObj.ToString(), Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                var res = client.SendAsync(httpRequest).Result;

                Console.WriteLine("Response :" + " - " + res.Content.ReadAsStringAsync().Result);
            }
            Run_Http2();
        }
    }
}

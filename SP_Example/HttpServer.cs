using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class HttpServer
    {
        static void Main(string[] args)
        {
            //1.Client에서 Server에 “http://127.0.0.1:8088/requestDate”로 요청하고,
            //Server는 현재 날짜와 시각을 Client로 응답하게 하시오. (요청 Method는 ‘GET’ 사용)

            //2.Client에서 Server에 접속하여 파일 목록을 json형태로 전송하는 프로그램을 작성하시오.
            //➢ Input 폴더의 파일 목록을 전송하여 Output폴더에 저장
            //- Client는 목록 전송 완료 후 종료
            //- Server는 목록을 수신하여 ‘수신시간.json’파일로 저장하고 다시 Client 접속 대기
            //-요청 Method는 ‘POST’사용
            //- Content Type은 “application / json” 사용
            void Run_Http()
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://127.0.0.1:8080/");
                listener.Start();
                while (true)
                {
                    var context = listener.GetContext();
                    //client로부터 요청온 내용을 context.request에서 까본다
                    Console.WriteLine(string.Format("Request({0}) : {1}", context.Request.HttpMethod, context.Request.Url));
                    DateTime dt = DateTime.Now;
                    String strRes = "";
                    //이게 1번
                    if (context.Request.HttpMethod == "GET")
                    {
                        strRes = dt.ToString();
                    }
                    //이게 2번
                    else if (context.Request.HttpMethod == "POST")
                    {
                        strRes = "(POST) " + dt.ToString();
                        System.IO.Stream body = context.Request.InputStream;
                        System.Text.Encoding encoding = context.Request.ContentEncoding;
                        System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                        string s = reader.ReadToEnd();
                        System.IO.Directory.CreateDirectory(".\\OutputHttp");
                        string fileName = string.Format($".\\OutputHttp\\{DateTime.Now.ToString("yyyyMMddHHmmss")}.json");
                        StreamWriter sw = new StreamWriter(fileName);
                        sw.WriteLine(s);
                        sw.Close();
                        body.Close();
                        reader.Close();
                    }
                    else
                    {

                    }

                    //client에 response 하고 싶은 것들을 context.response에 담는다
                    byte[] data = Encoding.UTF8.GetBytes(strRes);
                    context.Response.OutputStream.Write(data, 0, data.Length);
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
            }
            Run_Http();
        }
    }
}

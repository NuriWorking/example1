using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Question4_Server
{
    public class ReportHttpServer
    {
        HttpListener listener = new HttpListener();
        public ReportHttpServer()
        {
            listener.Prefixes.Add("http://127.0.0.1:8081/REPORT/");
        }

        public void DoHttpWork()
        {
            Console.WriteLine("Http Server Thread Start");
            listener.Start();

            while (true)
            {
                var context = listener.GetContext(); //context.Request.RawUrl: /REPORT/Manager1/230614

                Task.Run(() => MakeReport(context));

                //byte[] data = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                //context.Response.OutputStream.Write(data, 0, data.Length);
                //context.Response.StatusCode = 200;
                //context.Response.Close();
            }
        }
        private void MakeReport(HttpListenerContext context)
        {
            //Dictionary<string, ReportData> reportDatas = new Dictionary<string, ReportData>();
            List<ReportData> reportDatas = new List<ReportData>();
            string managerId = context.Request.RawUrl.Split('/')[2];
            string date = context.Request.RawUrl.Split('/')[3];

            DirectoryInfo di = new DirectoryInfo("..\\SERVER\\");
            FileInfo[] fi = di.GetFiles();
            foreach (FileInfo file in fi)
            {
                if (file.Name.Split('_')[2].Substring(0, 8).Equals(date))
                {
                    string line;
                    int lineNum = 0;
                    int errorNum = 0;
                    string validatorId = string.Empty;
                    //string errorCode = string.Empty;
                    StreamReader sr = new StreamReader(file.FullName);
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineData = line.Split('#');
                        validatorId = lineData[0];
                        if (!lineData[3].Equals("R1"))
                            errorNum++;
                        lineNum++;
                    }
                    ReportData reportData = new ReportData(validatorId, lineNum, errorNum);
                    reportDatas.Add(reportData);

                    sr.Close();
                }
            }
            //• 응답 Body : JSON 문자열 형식
            // 1) 응답할 Report가 존재할 경우
            // {“Result”:”Ok”,”ReportID”:”< Report ID >”,”Report”:”< Report >”}
            //                    2) 응답할 Report가 존재하지 않을 경우(< Date > 에 해당하는 Report 생성이 불가능할 경우)
            // {“Result”:”No Report”}
            string json = string.Empty;
            if (reportDatas.Count == 0)
                json = "{\"Result\":\"No Report\"}";
            else
            {
                string reportStr = MakeReportStr(reportDatas);
                json = "{\"Result\":\"Ok\", \"Report ID\":\"";
                Guid guid = new Guid();
                json += guid;
                json += "\", \"Report\":\"";
                json += reportStr;
                json += "\"}";
            }

            context.Response.ContentType = "application/json";
            context.Response.
        }

        private string MakeReportStr(List<ReportData> reportDatas)
        {
            //[검사원ID] [검사 카드 수] [비정상 카드 수]
            string rtn = string.Empty;
            foreach (var item in reportDatas)
            {
                rtn += $"{item.ValidatorId} {item.CheckCount} {item.ErrorCount}\n";
            }
            return rtn;
        }

        private void AddDataSafe(Dictionary<string, ReportData> dict, string key, ReportData value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Question2
{
    class ValidatorLauncher
    {
        static void Main(string[] args)
        {
            SetUserData("../CLIENT/INSPECTOR.TXT");
            while (true)
            {
                string inputIdPw = Console.ReadLine();
                if (SignIn(inputIdPw))
                {
                    Console.WriteLine("LOGIN SUCCESS");
                    break;
                }
                else
                {
                    Console.WriteLine("LOGIN FAIL");
                }
            }


            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("DONE"))
                {
                    inspDatas[busNow].InspEnd = DateTime.Now.ToString("yyyyMMddHHmmss");
                    OnInspection = false;
                }
                else if (input.Equals("LOGOUT"))
                {
                    break;
                }
                else
                {
                    if (!OnInspection)
                    {
                        OnInspection = true;
                        busNow = input;
                        InspData inspData = new InspData();
                        inspData.InspStart = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (inspDatas.Count == 0)
                            validatorStartTime = inspData.InspStart;
                        inspDatas.Add(input, inspData);
                    }
                    else
                    {
                        inspDatas[busNow].AddCardData(input);
                    }
                }
            }

            foreach (var item in inspDatas)
            {
                CalcInspectionCode(item);
            }
            DownloadFile(inspDatas);

            Console.ReadKey();
        }


        static string validatorId; //직원
        static string validatorStartTime; //업무시작
        static Dictionary<string, string> UserDict = new Dictionary<string, string>();
        static bool OnInspection = false;
        static Dictionary<string, InspData> inspDatas = new Dictionary<string, InspData>();
        static string busNow;

        static void SetUserData(string filename)
        {
            string line;
            StreamReader file = new StreamReader(filename);
            while ((line = file.ReadLine()) != null)
            {
                string[] idPw = line.Split(' ');
                //System.Console.WriteLine(pwDe);
                UserDict.Add(idPw[0], idPw[1]);
            }
            file.Close();
        }
        static bool SignIn(string input)
        {
            string[] inputIdPw = input.Split(' ');
            string inputId = inputIdPw[0];
            string inputPw = inputIdPw[1];
            if (UserDict.ContainsKey(inputId))
            {
                string encryptInputPw = CardUtility.passwordEncryption_SHA256(inputPw);
                if (encryptInputPw.Equals(UserDict[inputId]))
                {
                    validatorId = inputId;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Console.WriteLine("ID not Exist");
                return false;
            }
        }
        private static void CalcInspectionCode(KeyValuePair<string, InspData> data)
        {
            //R1 : 정상
            //R2 : 버스ID 정보가 다름
            //R3 : 승차 정보 없음
            //R4 : 승차 시각(3시간) 초과
            //if (검사버스id == 최근버스id 
            //&& 최근승하차=='n' 
            //&& 현재-최근승차시각<3h) then 정상승객

            string thisBusId = data.Key;
            InspData insp = data.Value;
            foreach (CardData item in insp.CardDatas)
            {
                if (thisBusId != item.RecentRideBusId)
                    item.InspectionCode = "R2";
                else if (item.RecentRideCode != "N")
                    item.InspectionCode = "R3";
                else if (Math.Abs(CardUtility.HourDiff(DateTime.Now.ToString("yyyyMMddHHmmss"), item.RecentRideTime)) >= 3)
                {
                    item.InspectionCode = "R4";
                }
                else
                    item.InspectionCode = "R1";
            }
        }
        private static void DownloadFile(Dictionary<string, InspData> inspDatas)
        {
            //• 검사 결과 파일은 다음 폴더와 파일 이름으로 저장한다. (상대경로 사용)
            // 폴더: .. / 검사원ID / (폴더가 없으면 생성해야 함)
            // 파일명: 검사원ID_검사시작시각.TXT
            //ex) .. / INSP_001 / INSP_001_20171023164832.TXT
            //• 검사 결과 파일의 내용은 다음과 같이 저장한다.
            //[검사원ID]#[버스ID]#[카드데이터]#[검사 결과 코드]#[검사시각]
            //ex) INSP_001#BUS_001# CARD_001BUS_001N20171019093610#R1#20171023164905
            // INSP_001#BUS_001# CARD_001BUS_001N20171019093610#R2#20171023165015

            string dir = $"../{validatorId}/";
            System.IO.Directory.CreateDirectory(dir);
            string fileName = $"{validatorId}_{validatorStartTime}.TXT";
            string finTxt = string.Empty;
            foreach (var item in inspDatas)
            {
                foreach (var item2 in item.Value.CardDatas)
                {
                    string txt = $"{validatorId}#{item.Key}#{item2.Meta}#{item2.InspectionCode}#{item2.CheckTime} \n";
                    finTxt += txt;
                }
            }
            using (StreamWriter sr = new StreamWriter(dir+fileName))
            {
                sr.Write(finTxt);
            }
        }

    }
}

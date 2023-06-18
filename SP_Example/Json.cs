using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class Json
    {
        static void Mainn(string[] args)
        {
            void JsonBasic()
            {
                //json 생성
                JObject json = new JObject();
                json["name"] = "John Doe";
                json["salary"] = 300100;
                string jsonstr = json.ToString();
                Console.WriteLine("json[\"name\"] : " + json["name"]);
                Console.WriteLine("Json : " + jsonstr);
                //string to json stringtojson
                JObject json2 = JObject.Parse(jsonstr);
                Console.WriteLine($"Name : {json2["name"]}, Salary : {json2["salary"]}");
            }
            //JsonBasic();

            //1.JSON Library를 활용하여 다음 데이터를 sample.json 파일로 저장하시오.
            //{
            //                "name":"spiderman",
            //"age":45,
            //"married":true,
            //"specialty":["martial art", "gun"],
            //"vaccine":{ "1st":"done","2nd":"expected","3rd":null},
            //"children": [{ "name":"spiderboy", "age":10}, { "name":"spidergirl", "age":8}],
            //"adress":null
            //}
            void Run_Json()
            {
                JObject jsonObj = new JObject();

                jsonObj.Add("name", "spiderman");
                jsonObj.Add("age", 45);
                jsonObj.Add("married", true);

                JArray jsonArr = new JArray();
                jsonArr.Add("martial art");
                jsonArr.Add("gun");
                jsonObj.Add("specialty", jsonArr);

                JObject jsonObj2 = new JObject();
                jsonObj2.Add("1st", "done");
                jsonObj2.Add("2nd", "expected");
                jsonObj2.Add("3rd", null);
                jsonObj.Add("vaccine", jsonObj2);

                JArray jsonArr2 = new JArray();
                jsonObj2 = new JObject();
                jsonObj2.Add("name", "spiderboy");
                jsonObj2.Add("age", 10);
                jsonArr2.Add(jsonObj2);

                jsonObj2 = new JObject();
                jsonObj2.Add("name", "spidergirl");
                jsonObj2.Add("age", 8);
                jsonArr2.Add(jsonObj2);

                jsonObj.Add("children", jsonArr2);
                jsonObj.Add("address", null);


                Console.WriteLine(jsonObj);

                File.WriteAllText(@"./sample.json", jsonObj.ToString());
            }
            //Run_Json();

            //2.sample.json 파일을 읽어서 다음과 같이 출력하시오.
            //name(age) : spiderman(45)
            //name(age) : spidergirl(8)
            void Run_Json2()
            {
                string sampleTxt = File.ReadAllText("./sample.json");
                JObject jo = JObject.Parse(sampleTxt);

                String name = jo["name"].ToString();
                int age = (int)jo["age"];
                Console.WriteLine("name(age) : " + name + "(" + age + ")");
                name = jo["children"][1]["name"].ToString(); //이름으로도, jarray의 경우 인덱스로도 찾을 수 있음!
                age = (int)jo["children"][1]["age"];
                Console.WriteLine("name(age) : " + name + "(" + age + ")");
            }
            //Run_Json2();

            //3.sample.json 파일을 읽은 후, 첫 번째 level의 Value Type을 알아내어
            //다음과 같이 출력하시오
            //Key : name / Value Type: String
            //Key : age / Value Type: Integer
            //...
            //Key: address / Value Type: Null
            void Run_Json3()
            {
                string sampleTxt = File.ReadAllText("./sample.json");
                JObject jo = JObject.Parse(sampleTxt);
                foreach (KeyValuePair<string, JToken> item in jo)
                {
                    Console.WriteLine($"Key: {item.Key}/ Value Type: {item.Value.Type}");
                }
            }
            Run_Json3();
        }
    }
}

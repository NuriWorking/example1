using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class Dictionary
    {
        static void Mainn(string[] args)
        {
            void DictionaryBasic()
            {
                Dictionary<string, string> m = new Dictionary<string, string>();
                m.Add("kit@gmail.com", "Michael Knight");
                m.Add("knife@gmail.com", "Mac Guyver");
                m.Add("superman@gmail.com", "Clark Kent");
                m.Add("batman@gmail.com", "Bruce Wayne");
                m.Add("ironman@gmail.com", "Tony Stark");
                foreach (KeyValuePair<string, string> items in m)
                {
                    Console.WriteLine(items.Key + " : " + items.Value);
                }
                Console.WriteLine();
                m.Remove("superman@gmail.com");
                foreach (KeyValuePair<string, string> items in m)
                {
                    Console.WriteLine(items.Key + " : " + items.Value);
                }
                Console.WriteLine();
                m["batman@gmail.com"] = "Robin";
                foreach (KeyValuePair<string, string> items in m)
                {
                    Console.WriteLine(items.Key + " : " + items.Value);
                }
            }
            
            //DS_Sample2.csv에는 2017년 직원들의 프로젝트 별 투입 MM가 월별로 저장되어 있다.
            //Map을 이용하여 사원들의 프로젝트 별 MM의 합계와 총 합계를 출력하시오.
            void Run_Dictionary()
            {
                Dictionary<string, Effort> effortDict = new Dictionary<string, Effort>();
                string line;
                using (StreamReader file = new StreamReader("./DS_Sample2.csv"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        string[]tmp= line.Split(',');
                        string key = tmp[1];
                        if (effortDict.ContainsKey(key))
                        {
                            effortDict[key].AddA(decimal.Parse(tmp[3]));
                            effortDict[key].AddB(decimal.Parse(tmp[4]));
                            effortDict[key].AddC(decimal.Parse(tmp[5]));
                        }
                        else
                        {
                            Effort newE = new Effort(tmp[2], decimal.Parse(tmp[3]), decimal.Parse(tmp[4]), decimal.Parse(tmp[5]));
                            effortDict.Add(tmp[1], newE);
                        }
                    }
                }
                foreach (var item in effortDict)
                {
                    Console.WriteLine($"{item.Key}\t{item.Value.Name}\t{item.Value.AProject}\t{item.Value.BProject}\t{item.Value.CProject}\t=>{item.Value.AProject+ item.Value.BProject + item.Value.CProject}");
                }
            }
            Run_Dictionary();
        }
    }
    public class Effort
    {
        public Effort(string name, decimal aProject, decimal bProject, decimal cProject)
        {
            Name = name;
            AProject = aProject;
            BProject = bProject;
            CProject = cProject;
        }

        public string Name { get; set; }
        public decimal AProject { get; set; }
        public decimal BProject { get; set; }
        public decimal CProject { get; set; }

        public void AddA(decimal input)
        {
            AProject += input;
        }
        public void AddB(decimal input)
        {
            BProject += input;
        }
        public void AddC(decimal input)
        {
            CProject += input;
        }
    }

}

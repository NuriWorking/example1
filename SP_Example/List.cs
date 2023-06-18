using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class List
    {
        static void Mainn(string[] args)
        {
            void ListBasic()
            {
                List<string> al = new List<string>();
                al.Add("Michael Knight");
                al.Add("Mac Guyver");
                al.Add("Clark Kent");
                al.Add("Bruce Wayne");
                al.Add("Tony Stark");
                foreach (string name in al)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();
                al.Remove("Clark Kent");
                for (int i = 0; i < al.Count; i++)
                {
                    Console.WriteLine(al[i]);
                }
                Console.WriteLine();
                al.Remove(al[0]);
                var enumerator = al.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Console.WriteLine(enumerator.Current);
                }
            }

            void ListBasic2()
            {
                List<string> numberList = new List<string>();
                numberList.Add("one");
                numberList.Add("two");
                numberList.Add("three");
                Console.WriteLine("Queue Count = {0}", numberList.Count);
                foreach (string number in numberList)
                {
                    Console.WriteLine(number);
                }
                Console.WriteLine("Deque '{0}'", numberList[0]);
                numberList.RemoveAt(0);
                Console.WriteLine("Peek : {0}", numberList[0]);
                Console.WriteLine("Contains(\"three\") = {0}", numberList.Contains("three"));
                numberList.Clear();
                Console.WriteLine("Queue Count = {0}", numberList.Count);
            }
            
            void ListSort()
            {
                List<string> al = new List<string>();
                al.Add("Michael Knight"); al.Add("Mac Guyver");
                al.Add("Clark Kent"); al.Add("Bruce Wayne"); al.Add("Tony Stark");
                al.Sort();// a~z 오름차순
                foreach (string name in al)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();

                //내림차순 (참고: x.CompareTo(y) 오름차순)
                al.Sort(delegate (string x, string y)
                {
                    return y.CompareTo(x); //CompareTo: 같으면 0, 왼<오면 -1, 오>왼이면 1
                });
                foreach (string name in al)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();

                //오름차순
                al.Sort((x, y) => x.CompareTo(y));
                foreach (string name in al)
                {
                    Console.WriteLine(name);
                }
                Console.WriteLine();

                al.Sort((string x, string y) => x.CompareTo(y));
                foreach (string name in al)
                {
                    Console.WriteLine(name);
                }
            }

            //List_Sample.txt에는 학생들의 성적 데이터가 저장되어 있다.
            //1. Console화면에서 ‘PRINT’를 입력하면 이름 순(오름차순)으로 출력하시오.
            //2. Console화면에서 ‘KOREAN’, ‘ENGLISH’, ‘MATH’를 입력하면 해당 과목
            //성적 순(내림차순)으로 출력해 보시오.
            //(성적이 동일할 경우에는 이름을 오름차순으로 정렬)
            //* ‘QUIT’을 입력하면 프로그램을 종료시킨다.
            void Run_List()
            {
                List<Grade> gradeList = new List<Grade>();
                string line;
                using (StreamReader file = new StreamReader(".\\List_Sample.txt"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] words = line.Split('\t');
                        Grade userGrade = new Grade(words[0], Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), Convert.ToInt32(words[3]));

                        gradeList.Add(userGrade);
                    }
                }
                while (true)
                {
                    string strLine = Console.ReadLine();
                
                    switch (strLine)
                    {
                        case "PRINT": //이름 오름차순
                            gradeList.Sort((x, y) => x.Name.CompareTo(y.Name));
                            break;
                        case "KOREAN": //국어 내림차순
                                gradeList.Sort((X, Y) => Y.Korean.CompareTo(X.Korean));
                            break;
                        case "ENGLISH": //영어 내림차순 (동점 시 이름 오름차순)
                            gradeList.Sort((x, y) =>
                            {
                                if (x.English == y.English)
                                    return x.Name.CompareTo(y.Name);
                                else
                                    return y.English.CompareTo(x.English);
                            }
                            );
                            break;
                        case "MATH":
                            gradeList.Sort(compare);
                            break;
                        case "QUIT": 
                            return; //종료~
                        default:
                            break;
                    }
                    foreach (var item in gradeList)
                    {
                        Console.WriteLine($"{item.Name} {item.Korean} {item.English} {item.Math}");
                    }
                }
            }            
            Run_List();
        }
        static int compare(Grade x, Grade y)//수학 내림차순 (동점 시 이름 오름차순)
        {
            if (x.Math == y.Math)
                return x.Name.CompareTo(y.Name);
            else
                return y.Math - x.Math;
        }
    }

    public class Grade
    {
        private String name;
        private int korean;
        private int english;
        private int math;

        public Grade(string str, int k, int e, int m)
        {
            Name = str;
            Korean = k;
            English = e;
            Math = m;
        }

        public string Name { get => name; set => name = value; }
        public int Korean { get => korean; set => korean = value; }
        public int English { get => english; set => english = value; }
        public int Math { get => math; set => math = value; }
    }
}

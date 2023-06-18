using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP_Example
{
    class Threading
    {
        static void Mainn(string[] args)
        {
            //외부 프로세스 실행
            string getProcessOutput(string fileName, string args)
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = fileName;
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.CreateNoWindow = true;
                start.Arguments = args;
                Process process = Process.Start(start);
                StreamReader reader = process.StandardOutput;
                return reader.ReadLine();
            }
            //string output = getProcessOutput("add_2sec.exe", "2 3");
            //Console.WriteLine(output);

            void Threading()
            {
                Worker workerObject1 = new Worker();
                Thread workerThread1 = new Thread(workerObject1.DoWork);
                // Start the worker thread.
                workerThread1.Start();
                workerThread1.Join();
            }
            //Threading();

            void Run_Thread()
            {
                //Thread 2개를 만든 후, Main함수와 Thread 2개에서 동시에 0부터 9까지 출력하시오.
                //어디서 출력하였는지 구분할 수 있게 숫자 앞에[Main], [Thread1], [Thread2] 표시하시오.
                Thread t1 = new Thread(() =>
                {
                    PrintNum("[Thread1]");
                }); 
                Thread t2 = new Thread(() =>
                {
                    PrintNum("[Thread2]");
                });
                t1.Start();
                t2.Start();
                PrintNum("[Main]");
                t1.Join();
                t2.Join();
            }
            //Run_Thread();

            void PrintNum(string tName)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"{tName} {i}");
                    Thread.Sleep(10);
                }
            }

            void Run_Thread2()
            {
                //NUM.TXT에 저장되어 있는 5쌍의 숫자들을 add_2sec.exe를 통해 덧셈을 실행시킨 후, 각
                //각의 결과들을 모두 출력하시오.
                //[조건]
                //- 전체 실행 시간은 5초 이내
                //-결과의 출력 순서는 상관없음
                //- 실행 시작과 끝에 현재시각 출력
                Console.WriteLine("Start - " + DateTime.Now);
                
                string[] lines = File.ReadAllLines("./NUM.TXT");
                List<Thread> threadList = new List<Thread>(); //join해주기 위해 list<thread>에 넣음!
                
                foreach (var line in lines)
                {
                    Thread t = new Thread(() =>
                    {
                        string[] splited = line.Split(' ');
                        ProcessStartInfo start = new ProcessStartInfo();
                        start.FileName = "./add_2sec.exe";
                        start.UseShellExecute = false;
                        start.RedirectStandardOutput = true;
                        start.CreateNoWindow = true;
                        start.Arguments = line;
                        Process process = Process.Start(start);
                        StreamReader reader = process.StandardOutput;
                        Console.WriteLine($"{splited[0]} + {splited[0]} = {reader.ReadLine()}");
                    });
                    t.Start();
                    threadList.Add(t);
                }
                foreach (var item in threadList)
                {
                    item.Join();
                }

                Console.WriteLine("End - " + DateTime.Now);
            }
            Run_Thread2();
        }
    }
    public class Worker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            Console.WriteLine("Thread is running...");
        }
    }
}

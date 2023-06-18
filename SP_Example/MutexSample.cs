using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP_Example
{
    class MutexSample
    {
        static void Mainn(string[] args)
        {
            void MutexSample()
            {
                MutexWorker workerObject1 = new MutexWorker();
                Thread workerThread1 = new Thread(workerObject1.DoWork);
                workerThread1.Start();
                workerThread1.Join();

            }

            void Run_Mutex()
            {
                //Mutex를 사용하여 Main과 2개의 Thread함수에서 다음과 같이 1~30까지 숫자를 연속으로
                //출력하게 하시오.
                MutexWorker workerObject1 = new MutexWorker(); //=>하나의 자원 class
                Thread t1 = new Thread(()=> workerObject1.DoWork2("[T1]"));
                Thread t2 = new Thread(() => workerObject1.DoWork2("[T2]"));
                t1.Start();
                t2.Start();
                workerObject1.DoWork2("[M]");
                t1.Join();
                t2.Join();
            }
            Run_Mutex();
        }
    }
    public class MutexWorker
    {
        private static Mutex mut = new Mutex();
        public void DoWork()
        {
            mut.WaitOne();
            for (int i = 1; i <= 30; i++)
            {
                Console.Write(i);
            }
            mut.ReleaseMutex();
        }
        public void DoWork2(string tName)
        {
            mut.WaitOne();
            Console.WriteLine(tName);
            for (int i = 1; i <= 30; i++)
            {
                Console.Write(i);
            }
            Console.WriteLine();
            mut.ReleaseMutex();
        }
    }
}

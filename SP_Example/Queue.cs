using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Example
{
    class Queue
    {
        static void Mainn(string[] args)
        {
            void QueueBasic2()
            {
                Queue<string> numberQ = new Queue<string>();
                numberQ.Enqueue("one");
                numberQ.Enqueue("two");
                numberQ.Enqueue("three");
                Console.WriteLine("Queue Count = {0}", numberQ.Count);
                foreach (string number in numberQ)
                {
                    Console.WriteLine(number);
                }
                Console.WriteLine("Deque '{0}'", numberQ.Dequeue());
                Console.WriteLine("Peek : {0}", numberQ.Peek());
                Console.WriteLine("Contains(\"three\") = {0}", numberQ.Contains("three"));
                numberQ.Clear();
                Console.WriteLine("Queue Count = {0}", numberQ.Count);
            }

            //입력 메시지(문자열)를 저장하는 Queue들을 작성하시오.
            void Run_Queue()
            {
                //key: queue name
                Dictionary<string, MsgQueue> queDict = new Dictionary<string, MsgQueue>();
                while (true)
                {
                    string line = Console.ReadLine();
                    string[] words = line.Split(' ');
                    string command = words[0];
                    string queName = words[1];
                    switch (command)
                    {
                        case "CREATE":
                            Console.WriteLine(QCreate(queDict, queName, int.Parse(words[2])));
                            break;
                        case "ENQUEUE":
                            Console.WriteLine(QEnqueue(queDict, queName, words[2]));
                            break;
                        case "DEQUEUE":
                            Console.WriteLine(QDequeue(queDict, queName));
                            break;
                        case "GET":
                            Console.WriteLine(QGet(queDict, queName));
                            break;
                        case "SET":
                            Console.WriteLine(QSet(queDict, queName, int.Parse(words[2])));
                            break;
                        case "DEL":
                            Console.WriteLine(QDel(queDict, queName, int.Parse(words[2])));
                            break;
                    }
                }
            }
            Run_Queue();

            string QCreate(Dictionary<string, MsgQueue> queDict, string queName, int queSize)
            {
                //- CREATE < Queue Name > < Queue Size >
                //: Queue Name으로 Queue생성, 정상 생성 시 “Queue Created” 출력
                //: Queue Name의 Queue가 이미 존재하는 경우 “Queue Exist” 출력
                if (queDict.ContainsKey(queName))
                {
                    return "Queue Exist";
                }
                MsgQueue q = new MsgQueue(queSize);
                queDict.Add(queName, q);
                return "Queue Created";
            }
            string QEnqueue(Dictionary<string, MsgQueue> queDict, string queName, string message)
            {
                //- ENQUEUE < Queue Name > < Message >
                //: Queue Name의 Queue에 Message저장, 저장 시 고유 Id값을 생성하여 함께 저장
                //: Queue Size개의 데이터가 이미 저장된 경우 “Queue Full”출력, 정상인 경우 “Enqueued” 출력
                return queDict[queName].MsgEnqueue(message);
            }
            string QDequeue(Dictionary<string, MsgQueue> queDict, string queName)
            {
                //- DEQUEUE < Queue Name >
                //: Queue Name의 Queue에 가장 먼저 저장된 Message와 Message Id를 출력하고, 해당 메시지 삭제
                //: Queue가 비어 있다면 “Queue Empty” 출력
                return queDict[queName].MsgDequeue();
            }
            string QGet(Dictionary<string, MsgQueue> queDict, string queName)
            {
                //- GET < Queue Name >
                //: Queue Name의 Queue에 가장 먼저 저장된 Message와 Message Id를 출력
                //: 해당 Message는 Queue에서 삭제되지 않지만, 다시 GET할 수는 없음
                return queDict[queName].MsgGet();
            }
            string QSet(Dictionary<string, MsgQueue> queDict, string queName, int id)
            {
                //- SET < Queue Name > < Message Id >
                //: Queue Name과 Message Id에 해당하는 Message를 다시 GET할 수 있게 세팅
                //: 세팅 성공 시 “Msg Set”, 실패 시 “Set Fail”출력
                return queDict[queName].MsgSet(id);
            }
            string QDel(Dictionary<string, MsgQueue> queDict, string queName, int id)
            {
                //- DEL < Queue Name > < Message Id >
                //: Queue에서 Message Id에 해당하는 Message 삭제, 삭제 성공 시 “Deleted” 출력
                //: 삭제 실패 시 “Not Deleted” 출력
                return queDict[queName].MsgDel(id);
            }
        }

    }
    public class MsgQueue
    {
        private int size;
        private int seqNo;

        // id - (status, msg)
        private SortedDictionary<int, List<string>> dicMsg;

        public MsgQueue(int size)
        {
            this.size = size;
            this.seqNo = 0;
            dicMsg = new SortedDictionary<int, List<string>>();
        }

        public string MsgEnqueue(string msg)
        {
            if (dicMsg.Count == size)
                return "Queue Full";

            List<string> listMsg = new List<string>();
            listMsg.Add("A"); // status : available
            listMsg.Add(msg); // message
            dicMsg.Add(seqNo++, listMsg);

            return "Enqueued";
        }

        public string MsgDequeue()
        {
            if (dicMsg.Count == 0)
                return "Queue Empty";

            int key = dicMsg.First().Key; //딕셔너리의 첫번째

            string res = dicMsg[key][1] + "(" + key + ")";

            dicMsg.Remove(key);

            return res;
        }

        public string MsgGet()
        {
            if (dicMsg.Count > 0)
                foreach (var item in dicMsg)
                {
                    if (item.Value[0] == "A")
                    {
                        item.Value[0] = "U";
                        return item.Value[1] + "(" + item.Key + ")";
                    }
                }

            return "No Msg";
        }

        public string MsgSet(int id)
        {
            if (dicMsg.Count > 0)
            {
                if (dicMsg.ContainsKey(id))
                {
                    dicMsg[id][0] = "A";
                    return "Msg Set";
                }
            }

            return "Set Fail";
        }

        public string MsgDel(int id)
        {
            if (dicMsg.Count > 0)
            {
                if (dicMsg.ContainsKey(id))
                {
                    dicMsg.Remove(id);
                    return "Deleted";
                }
            }

            return "Not Deleted";
        }
    }
}

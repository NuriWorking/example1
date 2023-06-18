using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP_Example
{
    class SocketServer
    {
        static Socket listener;
        static string receiveFolder = "./ServerFiles";
        static void Mainn(string[] args)
        {
            //클라이언트 접속 시 test 문자열 전송
            void SocketServerSample()
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 9090);
                Socket listenerSocket = new Socket(
                AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            
                try
                {
                    listenerSocket.Bind(localEndPoint);
                    listenerSocket.Listen(10);
                    Socket handler = listenerSocket.Accept();
                    byte[] msg = Encoding.ASCII.GetBytes("test");
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.ReadKey();
            }
            //SocketServerSample();

            //1.Client에서 Server에 접속하면 Server는 현재 날짜와 시각을 Client로 전송하고,
            //Client는 전송 받은 값을 출력하시오. 
            void Run_Socket()
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint endPoint = new IPEndPoint(ip, 9090);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Bind(endPoint);
                    socket.Listen();
                    while (true)
                    {
                        Socket handler = socket.Accept();
                        byte[] msg = Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        handler.Send(msg);

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.Read();
            }
            //Run_Socket();

            //2.Client에서 Server에 접속하여 파일을 전송하는 프로그램을 작성하시오.
            //➢ ClientFiles 폴더의 모든 파일을 전송하여 ServerFiles폴더에 저장
            //- Client는 파일 전송 완료 후 종료
            //- Server는 파일을 수신 완료하고 다시 Client 접속 대기
            //- Server는 ‘QUIT’입력을 받으면 종료
            void Run_Socket2()
            {
                Console.WriteLine("Run_Socket2");
                Directory.CreateDirectory(receiveFolder);
                
                Thread t = new Thread(()=> ServerWork());
                t.Start();
                
                while (true)
                {
                    string line = Console.ReadLine();
                    if (line.Equals("QUIT"))
                    {
                        listener.Close();
                        break;
                    }
                }
                t.Join();
            }
            Run_Socket2();

            void ServerWork()
            {
                const int BUF_SIZE = 4096;
                // Data buffer for incoming data.
                byte[] bytes = new Byte[BUF_SIZE];

                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint endPoint = new IPEndPoint(ip, 9090);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                listener.Bind(endPoint);
                listener.Listen(10);

                try
                {
                    while (true)
                    {
                        Socket handler = listener.Accept();

                        NetworkStream ns = new NetworkStream(handler);
                        BinaryReader br = new BinaryReader(ns);
                        FileStream fs = null;
                        try
                        {
                            string filename;
                            // 파일이름 수신
                            while ((filename = br.ReadString()) != null)
                            {
                                Console.WriteLine(filename);
                                // 파일크기 수신
                                int length = (int)br.ReadInt64();

                                fs = new FileStream(receiveFolder + "/" + filename, FileMode.Create);
                                while (length > 0)
                                {
                                    // 파일내용 수신
                                    int nReadLen = br.Read(bytes, 0, Math.Min(BUF_SIZE, length)); //스트림의 마지막에는 버퍼사이즈보다 작을테니까~
                                    fs.Write(bytes, 0, nReadLen);
                                    length -= nReadLen;
                                }
                                fs.Close();
                                Console.WriteLine(filename + " is received.");
                            }
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                listener.Close();
            }
        }
    }
}

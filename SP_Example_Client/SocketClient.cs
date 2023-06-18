using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SP_Example_Client
{
    class SocketClient
    {
        static void Mainn(string[] args)
        {
            //서버 접속 후 메시지 수신
            void SocketClientSample()
            {
                byte[] bytes = new byte[1024];
                try
                {
                    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9090);
                    Socket senderSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        senderSocket.Connect(remoteEP);
                        int bytesRec = senderSocket.Receive(bytes);//버퍼에 담음
                        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        senderSocket.Shutdown(SocketShutdown.Both);
                        senderSocket.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.ReadKey();
            }
            //SocketClientSample();

            //1.Client에서 Server에 접속하면 Server는 현재 날짜와 시각을 Client로 전송하고,
            //Client는 전송 받은 값을 출력하시오. 
            void Run_Socket()
            {
                byte[] bytes = new byte[1024];
                try
                {
                    IPAddress ip = IPAddress.Parse("127.0.0.1");
                    IPEndPoint remoteEP = new IPEndPoint(ip, 9090);
                    Socket sender = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        sender.Connect(remoteEP); 
                        
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));

                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.Read();
            }
            //Run_Socket();

            void Run_Socket2()
            {
                Console.WriteLine("Run_Socket2");
                const int BUF_SIZE = 4096;
                // Data buffer for incoming data.
                byte[] bytes = new byte[BUF_SIZE];

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9090);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    NetworkStream ns = new NetworkStream(sender);
                    BinaryWriter bw = new BinaryWriter(ns);

                    DirectoryInfo di = new DirectoryInfo("./ClientFiles");
                    FileInfo[] fiArr = di.GetFiles();
                    foreach (FileInfo infoFile in fiArr)
                    {
                        Console.WriteLine(infoFile.FullName);
                        // 파일이름 전송
                        bw.Write(infoFile.Name);
                        long lSize = infoFile.Length;

                        // 파일크기 전송
                        bw.Write(lSize);

                        // 파일내용 전송
                        FileStream fs = new FileStream(infoFile.FullName, FileMode.Open);
                        while (lSize > 0)
                        {
                            int nReadLen = fs.Read(bytes, 0, Math.Min(BUF_SIZE, (int)lSize));
                            bw.Write(bytes, 0, nReadLen);
                            lSize -= nReadLen;
                        }
                        fs.Close();
                    }
                    bw.Close();
                    ns.Close();

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.ReadKey();
            }
            Run_Socket2();
        }
    }
}

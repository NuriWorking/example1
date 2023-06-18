using System;
using System.Threading;
using System.Threading.Tasks;

namespace Question3_Server
{
    class ServerLauncher
    {
        static void Main(string[] args)
        {

            // Sokcet Server Start
            CardSocketServer cardServer = new CardSocketServer();
            //Thread socketServerThread = new Thread(cardServer.DoSocketWork);
            //socketServerThread.Start();
            Task.Run(() => cardServer.DoSocketWork());

            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("QUIT"))
                {
                    cardServer.listener.Close();
                }
                    break;
            }
            //socketServerThread.Join();
            
        }
    }
}

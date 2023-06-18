using System;
using System.Threading;
using System.Threading.Tasks;

namespace Question4_Server
{
    class ServerLauncher
    {
        static void Main(string[] args)
        {
            ReportHttpServer httpServer = new ReportHttpServer();
            //Task.Run(() => httpServer.DoHttpWork());

            Thread socketServerThread = new Thread(httpServer.DoHttpWork);
            socketServerThread.Start();
            socketServerThread.Join()
;        }
    }
}

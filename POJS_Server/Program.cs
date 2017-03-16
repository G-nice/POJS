using System;
using System.IO;
using System.Threading;

namespace POJS_Server
{
    public class Program
    {
        public static int Main(String[] args)
        {
            if (!LoadConfig.load())
                return 1;

            HttpServer httpServer;
            if (LoadConfig.config.ServerIP.Length != 0 && LoadConfig.config.ServerPort.Length != 0)
            {
                httpServer = new MyHttpServer(LoadConfig.config.ServerIP, Convert.ToInt16(LoadConfig.config.ServerPort));
                Console.WriteLine("Star server at {0} : {1}\n", LoadConfig.config.ServerIP, LoadConfig.config.ServerPort);
            }
            else
            {
                httpServer = new MyHttpServer("127.0.0.1", 8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();

            /*
            while(true)
            {
                if (Console.ReadLine().Equals("exit"))
                {
                    httpServer.is_active = false;
                    return 0;
                }
                    //Application;
            }
            */
            return 0;
        }

    }
}

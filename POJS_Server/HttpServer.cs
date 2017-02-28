using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;

namespace POJS_Server
{
    public abstract class HttpServer
    {

        protected int port;
        protected string ip_string;
        public bool is_active = true;
        TcpListener listener;

        public HttpServer(string ip_string = "127.0.0.1", int port = 8080)
        {
            this.ip_string = ip_string;
            this.port = port;
        }

        public void listen()
        {
            //listener = new TcpListener(port);
            IPAddress ip = IPAddress.Parse(ip_string);
            listener = new TcpListener(ip, port);

            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                Console.WriteLine("{0}与{1}连接", DateTime.Now.ToString(), s.Client.RemoteEndPoint.ToString());
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }

        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
}

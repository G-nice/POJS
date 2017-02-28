using System;
using System.IO;
using System.Threading;

namespace POJS_Server
{
    public class MyHttpServer : HttpServer
    {
        public MyHttpServer(string ip, int port)
            : base(ip, port)
        {
        }
        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}\n", p.http_url);
            p.writeSuccess();
            switch(p.http_url)
            {
                case "/" :
                    p.outputStream.WriteLine("<!DOCTYPE html>");
                    p.outputStream.WriteLine("<html><body><h1>Programming Online Judge Ssystem</h1>");
                    //p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
                    //p.outputStream.WriteLine("url : {0}", p.http_url);
                    p.outputStream.WriteLine("<form method=post action=/form>");
                    p.outputStream.WriteLine("<input type=text name=sorcecode value=><p>");
                    p.outputStream.WriteLine("<input type=text name=input value=><p>");
                    p.outputStream.WriteLine("<input type=text name=output value=><p>");
                    p.outputStream.WriteLine("<input type=submit name=submit value=提交>");
                    p.outputStream.WriteLine("</form>");
                    break;
                case "/check":
                    p.outputStream.WriteLine("OK");
                    break;
                default:
                    p.outputStream.WriteLine("<html><body><h1>test server</h1>");
                    p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
                    p.outputStream.WriteLine("url : {0}", p.http_url);
                    p.outputStream.WriteLine("<form method=post action=/form>");
                    p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
                    p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
                    p.outputStream.WriteLine("</form>");
                    break;
            }

        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            CodeJudge cj = new CodeJudge();
            Console.WriteLine("POST request: {0}\n", p.http_url);
            string[] datas = inputData.ReadToEnd().Split(new string[] { "#&" }, StringSplitOptions.None);
            string result = "";

            //Console.WriteLine("datas length: {0}", datas.Length);

            p.writeSuccess();
            switch(p.http_url)
            {
                case "/submit" :
                    if (cj.Complie(datas[0]))
                        switch (cj.JudgeProgeam(datas[1], datas[2]))
                        {
                            case ExecStatus.ACCEPT:
                            result = "接受";
                                break;
                            case ExecStatus.TIMEOUT:
                                result = "运行超时";
                                break;
                            case ExecStatus.WRONG:
                            default:
                                result = "结果错";
                                break;

                        }
                    else
                        result = "编译错";
                    p.outputStream.WriteLine(result);
                    break;

                default:
                    p.outputStream.WriteLine("<!DOCTYPE html>");
                    p.outputStream.WriteLine("<html><body><h1>test server</h1>");
                    p.outputStream.WriteLine("<a href=/test>return</a><p>");
                    p.outputStream.WriteLine("postbody: <pre>{0}</pre>", datas[0]);
                //p.outputStream.WriteLine("\r\n some data\r\n");
                    break;

            }
        }

    }

    public class TestMain
    {
        public static int Main(String[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) == 2)
            {
                httpServer = new MyHttpServer(args[0], Convert.ToInt16(args[1]));
                Console.WriteLine("Star server at {0} : {1}\n", args[0], args[1]);
            }
            else
            {
                httpServer = new MyHttpServer("127.0.0.1", 8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();

            while(true)
            {
                if (Console.ReadLine().Equals("exit"))
                {
                    httpServer.is_active = false;
                    return 0;
                }
                    //Application;
            }
            //return 0;
        }

    }
}

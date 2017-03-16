using System;
using System.IO;


namespace POJS_Server
{
    public class MyHttpServer : HttpServer
    {
        private static string indexContent = null;
        private static string judgeJs = null;
        private static byte[] favicon = null;

        public MyHttpServer(string ip, int port)
            : base(ip, port)
        {
            indexContent = File.ReadAllText(LoadConfig.config.ResourcesPath + "Pages\\" + "index.html");
            judgeJs = File.ReadAllText(LoadConfig.config.ResourcesPath + "Pages\\" + "judge.js");
            // FileInfo fileinfo = new FileInfo(LoadConfig.config.ResourcesPath + "favicon.png");
            // favicon = File.ReadAllBytes("Resources/favicon.png");
            // Console.WriteLine("FINISH INITIALIZE MYHTTPSERVER");
        }
        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}\n", p.http_url);
            switch(p.http_url)
            {
                case "/" :
                    /*
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
                    */
                    p.writeSuccess();
                    p.outputStream.Write(indexContent);
                    break;
                case "/favicon.ico":
                    p.writeFailure();
                    // p.writeSuccessPic();
                    // p.outputStream.Write(favicon);
                    // p.outputStream.Write("\r\n\r\n");
                    break;
                case "/resource/judge.js":
                    p.writeSuccess();
                    p.outputStream.Write(judgeJs);
                    break;
                case "/check":
                    p.writeSuccess();
                    p.outputStream.WriteLine("OK");
                    break;
                default:
                    p.writeFailure();
                    p.outputStream.WriteLine("Hello World!!! \n404 NOT FOUND");
                    //p.outputStream.WriteLine("<html><body><h1>test server</h1>");
                    //p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
                    //p.outputStream.WriteLine("url : {0}", p.http_url);
                    //p.outputStream.WriteLine("<form method=post action=/form>");
                    //p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
                    //p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
                    //p.outputStream.WriteLine("</form>");
                    break;
            }

        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            CodeJudge cj = new CodeJudge(LoadConfig.config.CompilerPath, LoadConfig.config.WorkPath);
            Console.WriteLine("POST request: {0}\n", p.http_url);
            string[] datas = inputData.ReadToEnd().Split(new string[] { "#$" }, StringSplitOptions.None);
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
                    p.writeFailure();
                    p.outputStream.WriteLine("ERROR");
                    //p.outputStream.WriteLine("<!DOCTYPE html>");
                    //p.outputStream.WriteLine("<html><body><h1>test server</h1>");
                    //p.outputStream.WriteLine("<a href=/test>return</a><p>");
                    //p.outputStream.WriteLine("postbody: <pre>{0}</pre>", datas[0]);
                    //p.outputStream.WriteLine("\r\n some data\r\n");
                    break;

            }
        }

    }

}

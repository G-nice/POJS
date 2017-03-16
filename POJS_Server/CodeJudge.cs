using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POJS_Server
{
    public enum ExecStatus {ACCEPT, TIMEOUT, WRONG };
    public class CodeJudge
    {
        string compilerPath = "";
        string workPath = "";
        string uniqueID = "";
        string result = null;

        string cfilePath = "";
        string generatedEXEPath = "";

        public CodeJudge()
        {
            //compilerPath = @"\gcc.exe";
            compilerPath = @".\MinGW\bin\gcc.exe";
            workPath = System.Environment.CurrentDirectory;
            //uniqueID = getThreadID().ToString() + DateTime.UtcNow.ToFileTimeUtc().ToString();
            uniqueID = GetThreadID().ToString();
            cfilePath = workPath + @"\" + uniqueID + ".c";
            generatedEXEPath = workPath + @"\" + uniqueID + ".exe";
            Console.WriteLine(uniqueID);
        }

        public CodeJudge(string _compilerPath, string _workPath)
        {
            compilerPath = _compilerPath;
            workPath = _workPath;
            uniqueID = GetThreadID().ToString();
            cfilePath = workPath + @"\" + uniqueID + ".c";
            generatedEXEPath = workPath + @"\" + uniqueID + ".exe";
        }

        public bool Complie(string sorceCode)
        {
            int exitcode = 0;
            if (File.Exists(cfilePath))
                File.Delete(cfilePath);
            FileStream cfile = new FileStream(cfilePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(cfile);
            sw.Write(sorceCode);
            sw.Close();
            cfile.Close();

            if (File.Exists(generatedEXEPath))
                File.Delete(generatedEXEPath);
            Process p = new Process();
            p.StartInfo.WorkingDirectory = workPath;
            p.StartInfo.FileName = compilerPath;
            p.StartInfo.Arguments = uniqueID + ".c -o " + uniqueID + ".exe -m32 -g3 -static-libgcc";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            Console.WriteLine("complie output: " + p.StandardOutput.ReadToEnd());
            Console.WriteLine("complie ExitCode: " + p.ExitCode);
            exitcode = p.ExitCode;
            p.Close();
            if (File.Exists(cfilePath))
                File.Delete(cfilePath);
            if (exitcode != 0)
                return false;
            return true;
        }

        public ExecStatus JudgeProgeam(string input, string output)
        {
            Process p = null;
            ExecStatus ret = ExecStatus.TIMEOUT;

            ReturnStringTaskHandler rsth = new ReturnStringTaskHandler(
                () => {
                    string result_tmp = null;
                    p = new Process();
                    p.StartInfo.WorkingDirectory = workPath;
                    p.StartInfo.FileName = generatedEXEPath;
                    p.StartInfo.UseShellExecute = false;
                    if(input != null)
                        p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    if (p.HasExited)
                        return result_tmp;
                    if (input != null)
                    {
                        p.StandardInput.AutoFlush = true;
                        p.StandardInput.WriteLine(input);
                        p.StandardInput.Flush();
                    }

                    p.WaitForExit();
                    result_tmp = p.StandardOutput.ReadToEnd();
                    Console.WriteLine("excute output: " + p.StandardOutput.ReadToEnd());
                    Console.WriteLine("excute ExitCode: " + p.ExitCode);
                    return result_tmp;
                }
                );

            try
            {
                this.result = Timeout.CallWithTimeout(rsth, 3000, delegate() { p.Kill(); }, false);
            }
            catch(TimeoutException e)
            {
                Console.WriteLine("捕获超时异常: " + e.Message);
                ret = ExecStatus.TIMEOUT;
            }
            finally
            {
                if(!p.HasExited)
                    p.Close();
                if (File.Exists(generatedEXEPath))
                    File.Delete(generatedEXEPath);

            }
            if (this.result != null)
            {
                if (!this.result.Equals(output))
                   ret = ExecStatus.WRONG;
                else
                    ret = ExecStatus.ACCEPT;
            }
            return ret;
        }



        private int GetThreadID()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        ~CodeJudge()
        {
            if (File.Exists(cfilePath))
                File.Delete(cfilePath);
            if (File.Exists(generatedEXEPath))
                File.Delete(generatedEXEPath);
        }
    }
}

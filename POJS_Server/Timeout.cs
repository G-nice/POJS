using System;
using System.Threading;

namespace POJS_Server
{
    public delegate string ReturnStringTaskHandler();

    class Timeout
    {
        /// <summary>
        /// 带超时执行任务
        /// </summary>
        /// <param name="things">需要执行的任务</param>
        /// <param name="timeoutMilliseconds">超时时间微秒</param>
        /// <param name="timeoutHandler">如果超时执行的操作</param>
        /// <param name="isForceKill">超时后是否强制abort线程</param>
        /// <returns></returns>
        public static string CallWithTimeout(ReturnStringTaskHandler things, int timeoutMilliseconds, Action timeoutHandler = null, bool isForceKill = false)
        {
            string ret = null;
            Thread threadToKill = null;

            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                ret = things.Invoke();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);

            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                if (timeoutHandler != null)
                    timeoutHandler.Invoke();
                if (isForceKill)
                    threadToKill.Abort();
                throw new TimeoutException();
            }
            return ret;
        }

    }
}

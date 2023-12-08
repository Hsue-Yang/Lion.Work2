using System;
using System.Threading;
using System.Threading.Tasks;

namespace LionTech.Utility.ERP
{
    public class RetryHelper
    {
        public static void GetPolicyRetry(Action action, int retryCount, int delayMilliseconds)
        {
            for (int i = 0; i <= retryCount; i++)
            {
                try
                {
                    action.Invoke();
                    return;
                }
                catch
                {
                    if (i < retryCount)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static TResult GetPolicyRetry<TResult>(Func<TResult> func, int retryCount, int delayMilliseconds)
        {
            for (int i = 0; i <= retryCount; i++)
            {
                try
                {
                    return func.Invoke();
                }
                catch
                {
                    if (i < retryCount)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return default;
        }

        public static async Task GetPolicyRetryAsync(Func<Task> func, int retryCount, int delaymilliSeconds)
        {
            for (int i = 0; i <= retryCount; i++)
            {
                try
                {
                    await func.Invoke();
                }
                catch
                {
                    if (i < retryCount)
                    {
                        await Task.Delay(delaymilliSeconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static async Task<TResult> GetPolicyRetryAsync<TResult>(Func<Task<TResult>> func, int retryCount, int delaymilliSeconds)
        {
            for (int i = 0; i <= retryCount; i++)
            {
                try
                {
                    return await func.Invoke();
                }
                catch
                {
                    if (i < retryCount)
                    {
                        await Task.Delay(delaymilliSeconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return default;
        }
    }
}
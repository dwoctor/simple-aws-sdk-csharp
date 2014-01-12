using AWS.SQS;
using AWS.SQS.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.Tests.Library
{
    public class SQSQueueTests : SQSTests
    {
        /// <summary>
        /// Tests Constructor
        /// </summary>
        /// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
        public Boolean Constructor()
        {
            String queueName = _queueName;
            try
            {
#if NET45
                var task = Task.Run(async () =>
                {
                    await SQSQueue.CreateQueueAsync(TestCredentials.Credentials, queueName);
                });
                task.Wait();
#else
                SQSQueue.CreateQueue(TestCredentials.Credentials, queueName);
#endif
                Thread.Sleep(1000);
            }
            catch (SQSQueueException) { }
            try
            {
                SQSQueue queue = new SQSQueue(TestCredentials.Credentials, queueName);
            }
            catch
            {
                return false;
            }
            try
            {
#if NET45
                var task = Task.Run(async () =>
                {
                    await SQSQueue.DeleteQueueAsync(TestCredentials.Credentials, queueName);
                });
                task.Wait();
#else
                SQSQueue.DeleteQueue(TestCredentials.Credentials, queueName);
#endif
                Thread.Sleep(1000);
            }
            catch (SQSQueueException) { }
            return true;
        }
    }
}

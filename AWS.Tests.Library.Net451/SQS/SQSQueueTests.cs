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
                    await SQSQueue.CreateQueueAsync(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, queueName);
                });
                task.Wait();
#else
                SQSQueue.CreateQueue(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, queueName);
#endif
                Thread.Sleep(1000);
            }
            catch (SQSQueueException) { }
            try
            {
                SQSQueue queue = new SQSQueue(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, queueName);
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
                    await SQSQueue.DeleteQueueAsync(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, queueName);
                });
                task.Wait();
#else
                SQSQueue.DeleteQueue(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, queueName);
#endif
                Thread.Sleep(1000);
            }
            catch (SQSQueueException) { }
            return true;
        }
    }
}

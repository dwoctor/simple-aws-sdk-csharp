using AWS.SQS;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.SQS
{
    [TestClass]
    public class SQSQueueTests : SQSTests
    {
        /// <summary>
        /// Tests Constructor, CreateQueue and DeleteQueue
        /// </summary>
        [TestMethod]
        public void ConstructorAndCreateAndDeleteQueue()
        {
            this.TestData.GenerateNewQueueName();
            SQSQueue.CreateQueue(this.TestData.Credentials, this.TestData.QueueName);
            Thread.Sleep(this.TestData.WaitTime);
            SQSQueue queue = new SQSQueue(this.TestData.Credentials, this.TestData.QueueName);
            SQSQueue.DeleteQueue(this.TestData.Credentials, this.TestData.QueueName);
        }

        /// <summary>
        /// Tests Constructor, CreateQueueAsync and DeleteQueueAsync
        /// </summary>
        [TestMethod]
        public async Task ConstructorAndCreateAndDeleteQueueAsync()
        {
            this.TestData.GenerateNewQueueName();
            await SQSQueue.CreateQueueAsync(this.TestData.Credentials, this.TestData.QueueName);;
            Thread.Sleep(this.TestData.WaitTime);
            SQSQueue queue = new SQSQueue(this.TestData.Credentials, this.TestData.QueueName);
            await SQSQueue.DeleteQueueAsync(this.TestData.Credentials, this.TestData.QueueName);
        }
    }
}

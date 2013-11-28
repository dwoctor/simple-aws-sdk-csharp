using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.SQS;
using AWS.SQS.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.SQS
{
    /// <summary>
    /// A SQSQueue
    /// </summary>
    public class SQSQueue
    {
        #region Fields
        /// <summary>
        /// AmazonSQS Client
        /// </summary>
        private AmazonSQSClient _sqs;

        /// <summary>
        /// The name of the queue
        /// </summary>
        private String _queueName = null;

        /// <summary>
        /// The url of the queue
        /// </summary>
        private String _queueUrl = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Upon construction of the queue
        /// </summary>
        public SQSQueue(String accesskey, String secretAccessKey, String queueName)
        {
            try
            {
                this._sqs = new AmazonSQSClient(accesskey, secretAccessKey);
                this.Name = queueName;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the queue
        /// </summary>
        public String Name
        {
            get
            {
                try
                {
                    return this._queueName;
                }
                catch (Exception error)
                {
                    throw SQSQueueException.Generator(error);
                }
            }
            internal set
            {
                try
                {
                    this._queueName = value;
#if NET45
                    var task = Task.Run(async () =>
                    {
                        GetQueueUrlResponse getQueueUrlResponse = await _sqs.GetQueueUrlAsync(new GetQueueUrlRequest() { QueueName = value });
                        this._queueUrl = getQueueUrlResponse.QueueUrl;
                    });
                    task.Wait();
#else
                    GetQueueUrlResponse getQueueUrlResponse = _sqs.GetQueueUrl(new GetQueueUrlRequest() { QueueName = value });
                    this._queueUrl = getQueueUrlResponse.QueueUrl;
#endif
                }
                catch (Exception error)
                {
                    throw SQSQueueException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The name of the queue
        /// </summary>
        public String QueueUrl
        {
            get
            {
                return this._queueUrl;
            }
            internal set
            {
                try
                {
                    this._queueUrl = value;
                    this._queueName = this._queueUrl.Substring(_queueUrl.LastIndexOf('/'));
                }
                catch (Exception error)
                {
                    throw SQSQueueException.Generator(error);
                }
            }
        }
        #endregion

        #region Methods

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a queue associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="queueName">The name of the table to create</param>
        public static SQSQueue CreateQueue(String accessKey, String secretAccessKey, String queueName)
        {
            try
            {
                AmazonSQSClient sqs = new AmazonSQSClient(accessKey, secretAccessKey);
                CreateQueueResponse createQueueResponse = sqs.CreateQueue(new CreateQueueRequest() { QueueName = queueName });
                return new SQSQueue(accessKey, secretAccessKey, createQueueResponse.QueueUrl);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a queue associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="queueName">The name of the queue</param>
        public static void DeleteQueue(String accessKey, String secretAccessKey, String queueName)
        {
            try
            {
                AmazonSQSClient sqs = new AmazonSQSClient(accessKey, secretAccessKey);
                GetQueueUrlResponse getQueueUrlResponse = sqs.GetQueueUrl(new GetQueueUrlRequest() { QueueName = queueName });
                DeleteQueueResponse deleteQueueRequest = sqs.DeleteQueue(new DeleteQueueRequest() { QueueUrl = getQueueUrlResponse.QueueUrl });
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Gets a list of all the queues associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        public static List<SQSQueue> GetListOfQueues(String accesskey, String secretAccessKey)
        {
            try
            {
                List<SQSQueue> queues = new List<SQSQueue>();
                AmazonSQSClient sqs = new AmazonSQSClient(accesskey, secretAccessKey);
                ListQueuesResponse listQueuesResponse = sqs.ListQueues(new ListQueuesRequest());
                foreach (var queueUrl in listQueuesResponse.QueueUrls)
                {
                    queues.Add(new SQSQueue(accesskey, secretAccessKey, queueUrl));
                }
                return queues;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(SQSMessage message)
        {
            try
            {
                this._sqs.SendMessage(new SendMessageRequest() { MessageBody = message.Body, QueueUrl = this._queueUrl });
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Sends multiple messages
        /// </summary>
        /// <param name="message">The messages to sent</param>
        public void SendMessages(List<SQSMessage> messages)
        {
            try
            {
                List<List<SendMessageBatchRequestEntry>> batchs = new List<List<SendMessageBatchRequestEntry>>();
                List<Int32> writeBatchIndexRequestCounts = new List<Int32>();
                Int32 batchIndex = 0;
                batchs.Add(new List<SendMessageBatchRequestEntry>());
                for (Int32 i = 0; i < messages.Count; i++)
                {
                    if (batchs[batchIndex].Count == 10)
                    {
                        batchIndex++;
                        batchs.Add(new List<SendMessageBatchRequestEntry>());
                    }
                    batchs[batchIndex].Add(new SendMessageBatchRequestEntry { Id = (batchs[batchIndex].Count + 1).ToString(), MessageBody = messages[i].Body });
                }
                foreach (var batch in batchs)
                {
                    this._sqs.SendMessageBatch(new SendMessageBatchRequest() { Entries = batch, QueueUrl = this._queueUrl });
                }
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Receive One Message
        /// </summary>
        /// <returns>SQSMessage</returns>
        public SQSMessage ReceiveMessage()
        {
            try
            {
                List<SQSMessage> messages = ReceiveMessages(1);
                if (messages.Count == 1)
                {
                    return messages[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Receive Multiple Messages
        /// </summary>
        /// <param name="maxNumberOfMessages">The maximum number of messages to Receive</param>
        /// <returns>SQSMessage</returns>
        public List<SQSMessage> ReceiveMessages(Int32 maxNumberOfMessages = 0)
        {
            try
            {
                List<SQSMessage> messages = new List<SQSMessage>();
                Boolean empty = false;
                do
                {
                    try
                    {
                        ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                        receiveMessageRequest.QueueUrl = this._queueUrl;
                        if (maxNumberOfMessages > 0)
                        {
                            receiveMessageRequest.MaxNumberOfMessages = maxNumberOfMessages;
                        }
                        ReceiveMessageResponse receiveMessageResponse = this._sqs.ReceiveMessage(receiveMessageRequest);
                        if (receiveMessageResponse.Messages.Count > 0)
                        {
                            foreach (Message message in receiveMessageResponse.Messages)
                            {
                                messages.Add(new SQSMessage(message));
                            }
                        }
                        else
                        {
                            empty = true;
                        }
                    }
                    catch { }
                }
                while (empty == false);
                return messages;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
            
        }

        /// <summary>
        /// Deletes a Message from the Queue
        /// </summary>
        /// <param name="message">The Message to Delete</param>
        public void DeleteMessage(SQSMessage message)
        {
            try
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
                deleteMessageRequest.QueueUrl = this._queueUrl;
                deleteMessageRequest.ReceiptHandle = message.ReceiptHandle;
                this._sqs.DeleteMessage(deleteMessageRequest);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Deletes a Messages from the Queue
        /// </summary>
        /// <param name="message">The Messages to Delete</param>
        public void DeleteMessages(List<SQSMessage> messages)
        {
            try
            {
                Int32 currentMessage = 0;
                for (int i = 0; i < messages.Count / 10; i++)
                {
                    DeleteMessageBatchRequest deleteMessageBatchRequest = new DeleteMessageBatchRequest();
                    deleteMessageBatchRequest.QueueUrl = this._queueUrl;
                    for (int j = 0; j < 10; j++)
                    {
                        deleteMessageBatchRequest.Entries.Add(new DeleteMessageBatchRequestEntry { Id = messages[currentMessage].MessageID, ReceiptHandle = messages[currentMessage].ReceiptHandle });
                        currentMessage++;
                    }
                    this._sqs.DeleteMessageBatch(deleteMessageBatchRequest);
                }
                messages.Clear();
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Clears all Messages from the Queue
        /// </summary>
        public void ClearQueue()
        {
            try
            {
                Boolean clear = false;
                do
                {
                    List<SQSMessage> messages = ReceiveMessages();
                    if (messages.Count > 0)
                    {
                        DeleteMessages(messages);
                    }
                    else
                    {
                        clear = true;
                    }

                }
                while (clear == false);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }
#endif
        #endregion

        #region Async
#if NET45
        /// <summary>
        /// Creates a queue associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="queueName">The name of the table to create</param>
        public static async Task<SQSQueue> CreateQueueAsync(String accessKey, String secretAccessKey, String queueName)
        {
            try
            {
                AmazonSQSClient sqs = new AmazonSQSClient(accessKey, secretAccessKey);
                CreateQueueResponse createQueueResponse = await sqs.CreateQueueAsync(new CreateQueueRequest() { QueueName = queueName });
                return new SQSQueue(accessKey, secretAccessKey, queueName);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a queue associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="queueName">The name of the queue</param>
        public static async Task DeleteQueueAsync(String accessKey, String secretAccessKey, String queueName)
        {
            try
            {
                AmazonSQSClient sqs = new AmazonSQSClient(accessKey, secretAccessKey);
                GetQueueUrlResponse getQueueUrlResponse = await sqs.GetQueueUrlAsync(new GetQueueUrlRequest() { QueueName = queueName });
                DeleteQueueResponse deleteQueueRequest = await sqs.DeleteQueueAsync(new DeleteQueueRequest() { QueueUrl = getQueueUrlResponse.QueueUrl });
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Gets a list of all the queues associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        public static async Task<List<SQSQueue>> GetListOfQueuesAsync(String accesskey, String secretAccessKey)
        {
            try
            {
                List<SQSQueue> queues = new List<SQSQueue>();
                AmazonSQSClient sqs = new AmazonSQSClient(accesskey, secretAccessKey);
                ListQueuesResponse listQueuesResponse = await sqs.ListQueuesAsync(new ListQueuesRequest());
                foreach (var queueUrl in listQueuesResponse.QueueUrls)
                {
                    queues.Add(new SQSQueue(accesskey, secretAccessKey, queueUrl));
                }
                return queues;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }


        /// <summary>
        /// Sends a message to the queue
        /// </summary>
        /// <param name="message">The message to send</param>
        public async void SendMessageAsync(SQSMessage message)
        {
            try
            {
                await this._sqs.SendMessageAsync(new SendMessageRequest() { MessageBody = message.Body, QueueUrl = this._queueUrl });
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Sends multiple messages to the queue
        /// </summary>
        /// <param name="message">The messages to sent</param>
        public async void SendMessagesAsync(List<SQSMessage> messages)
        {
            try
            {
                List<List<SendMessageBatchRequestEntry>> batchs = new List<List<SendMessageBatchRequestEntry>>();
                List<Int32> writeBatchIndexRequestCounts = new List<Int32>();
                Int32 batchIndex = 0;
                batchs.Add(new List<SendMessageBatchRequestEntry>());
                for (Int32 i = 0; i < messages.Count; i++)
                {
                    if (batchs[batchIndex].Count == 10)
                    {
                        batchIndex++;
                        batchs.Add(new List<SendMessageBatchRequestEntry>());
                    }
                    batchs[batchIndex].Add(new SendMessageBatchRequestEntry { Id = (batchs[batchIndex].Count + 1).ToString(), MessageBody = messages[i].Body });
                }
                foreach (var batch in batchs)
                {
                    await this._sqs.SendMessageBatchAsync(new SendMessageBatchRequest() { Entries = batch, QueueUrl = this._queueUrl });
                }
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Receive one message from the queue
        /// </summary>
        /// <returns>SQSMessage</returns>
        public async Task<SQSMessage> ReceiveMessageAsync()
        {
            try
            {
                List<SQSMessage> messages = await ReceiveMessagesAsync(1);
                if (messages.Count == 1)
                {
                    return messages[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Receive multiple messages from the queue
        /// </summary>
        /// <param name="maxNumberOfMessages">The maximum number of messages to Receive</param>
        /// <returns>SQSMessage</returns>
        public async Task<List<SQSMessage>> ReceiveMessagesAsync(Int32 maxNumberOfMessages = 0)
        {
            try
            {
                List<SQSMessage> messages = new List<SQSMessage>();
                Boolean empty = false;
                do
                {
                    try
                    {
                        ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                        receiveMessageRequest.QueueUrl = this._queueUrl;
                        if (maxNumberOfMessages > 0)
	                    {
		                    receiveMessageRequest.MaxNumberOfMessages = maxNumberOfMessages;
	                    }
                        ReceiveMessageResponse receiveMessageResponse = await this._sqs.ReceiveMessageAsync(receiveMessageRequest);
                        if (receiveMessageResponse.Messages.Count > 0)
                        {
                            foreach (Message message in receiveMessageResponse.Messages)
                            {
                                messages.Add(new SQSMessage(message));
                            }
                        }
                        else
                        {
                            empty = true;
                        }
                    }
                    catch { }
                }
                while (empty == false);
                return messages;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Delete one message from the queue
        /// </summary>
        /// <param name="message">The message to delete</param>
        public async void DeleteMessageAsync(SQSMessage message)
        {
            try
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
                deleteMessageRequest.QueueUrl = this._queueUrl;
                deleteMessageRequest.ReceiptHandle = message.ReceiptHandle;
                await this._sqs.DeleteMessageAsync(deleteMessageRequest);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Deletes multiple messages from the queue
        /// </summary>
        /// <param name="message">The messages to delete</param>
        public async void DeleteMessagesAsync(List<SQSMessage> messages)
        {
            try
            {
                Int32 currentMessage = 0;
                for (int i = 0; i < messages.Count / 10; i++)
                {
                    DeleteMessageBatchRequest deleteMessageBatchRequest = new DeleteMessageBatchRequest();
                    deleteMessageBatchRequest.QueueUrl = this._queueUrl;
                    for (int j = 0; j < 10; j++)
                    {
                        deleteMessageBatchRequest.Entries.Add(new DeleteMessageBatchRequestEntry { Id = messages[currentMessage].MessageID, ReceiptHandle = messages[currentMessage].ReceiptHandle });
                        currentMessage++;
                    }
                    await this._sqs.DeleteMessageBatchAsync(deleteMessageBatchRequest);
                }
                messages.Clear();
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Clears all messages from the queue
        /// </summary>
        public async void ClearQueueAsync()
        {
            try
            {
                Boolean clear = false;
                do
                {
                    List<SQSMessage> messages = await ReceiveMessagesAsync();
                    if (messages.Count > 0)
                    {
                        DeleteMessagesAsync(messages);
                    }
                    else
                    {
                        clear = true;
                    }

                }
                while (clear == false);
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }
#endif
        #endregion

        #endregion
    }
}
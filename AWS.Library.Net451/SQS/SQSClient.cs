using AWS.SQS;
using AWS.SQS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.SQS
{
    /// <summary>
    /// A SQSClient
    /// </summary>
    public class SQSClient
    {
        #region Fields
        /// <summary>
        /// Stores the AWS access key
        /// </summary>
        private String _accessKey;

        /// <summary>
        /// Stores the AWS secret access key
        /// </summary>
        private String _secretAccessKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a SQSClient
        /// </summary>
        public SQSClient(String accessKey, String secretAccessKey)
        {
            try
            {
                this._accessKey = accessKey;
                this._secretAccessKey = secretAccessKey;
                this.Queues = new List<SQSQueue>();
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Queues
        /// </summary>
        public List<SQSQueue> Queues { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Get a queue
        /// </summary>
        /// <param name="queueName">The name of the queue to get</param>
        /// <returns>SQSQueue</returns>
        [System.Runtime.CompilerServices.IndexerName("SQSClientIndex")]
        public SQSQueue this[String queueName]
        {
            get
            {
                try
                {
                    return this.Queues.Single(x => x.Name == queueName);
                }
                catch (Exception error)
                {
                    throw SQSQueueException.Generator(error);
                }
            }
        }

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a Queue
        /// </summary>
        /// <param name="queueName">The name of the queue to create</param>
        public SQSQueue CreateQueue(String queueName)
        {
            try
            {
                SQSQueue queue = SQSQueue.CreateQueue(this._accessKey, this._secretAccessKey, queueName);
                this.Queues.Add(queue);
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
                return queue;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a queue
        /// </summary>
        /// <param name="queueName">The name of the queue to delete</param>
        public void DeleteQueue(String queueName)
        {
            try
            {
                SQSQueue.DeleteQueue(this._accessKey, this._secretAccessKey, queueName);
                this.Queues.RemoveAll(x => x.Name == queueName);
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Gets all the queues
        /// </summary>
        public void GetQueues()
        {
            try
            {
                List<SQSQueue> queues = SQSQueue.GetListOfQueues(this._accessKey, this._secretAccessKey);
                foreach (var queue in queues)
                {
                    if (this.Queues.Any(x => x.Name == queue.Name) == false)
                    {
                        this.Queues.Add(queue);
                    }
                }
                foreach (var queue in this.Queues)
                {
                    if (this.Queues.Any(x => x.Name == queue.Name) == false)
                    {
                        this.Queues.RemoveAll(x => x.Name == queue.Name);
                    }
                }
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
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
        /// Creates a queue
        /// </summary>
        /// <param name="queueName">The name of the queue to create</param>
        public async Task<SQSQueue> CreateQueueAsync(String queueName)
        {
            try
            {
                SQSQueue queue = await SQSQueue.CreateQueueAsync(this._accessKey, this._secretAccessKey, queueName);
                this.Queues.Add(queue);
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
                return queue;
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a queue
        /// </summary>
        /// <param name="queueName">The name of the queue to delete</param>
        public async void DeleteQueueAsync(String queueName)
        {
            try
            {
                await SQSQueue.DeleteQueueAsync(this._accessKey, this._secretAccessKey, queueName);
                this.Queues.RemoveAll(x => x.Name == queueName);
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw SQSQueueException.Generator(error);
            }
        }

        /// <summary>
        /// Gets all the queues
        /// </summary>
        public async void GetQueuesAsync()
        {
            try
            {
                List<SQSQueue> queues = await SQSQueue.GetListOfQueuesAsync(this._accessKey, this._secretAccessKey);
                foreach (var queue in queues)
                {
                    if (this.Queues.Any(x => x.Name == queue.Name) == false)
                    {
                        this.Queues.Add(queue);
                    }
                }
                foreach (var queue in this.Queues)
                {
                    if (this.Queues.Any(x => x.Name == queue.Name) == false)
                    {
                        this.Queues.RemoveAll(x => x.Name == queue.Name);
                    }
                }
                this.Queues = this.Queues.OrderBy(x => x.Name).ToList();
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
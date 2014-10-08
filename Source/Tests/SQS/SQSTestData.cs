using System;
using System.Collections.Generic;
using AWS;
using AWS.DynamoDB;
using AWS.Tests;

namespace AWS.Tests.SQS
{
    /// <summary>
    /// DynamoDBTestData
    /// </summary>
    public class SQSTestData : TestData
    {

        #region Fields
        /// <summary>
        /// Stores the QueueName
        /// </summary>
        private String _queueName;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a DynamoDBTestData object
        /// </summary>
        public SQSTestData() : base()
        {
            GenerateNewQueueName();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get and Set the QueueName
        /// </summary>
        public String QueueName
        {
            get
            {
                return this._queueName;
            }
            set
            {
                this._queueName = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new QueueName
        /// </summary>
        /// <remarks>
        /// QueueName is a concatenation of various elements.
        /// 
        /// QueueName string format: {0}-{1}-{2}{3}{4}{5}{6}{7}{8}{9}
        /// 
        /// QueueName string data:
        /// 
        /// {0} TestQueue
        /// {1} Random Int32
        /// {2} DateTime.UtcNow.Year
        /// {3} DateTime.UtcNow.Month
        /// {4} DateTime.UtcNow.Day
        /// {5} DateTime.UtcNow.Hour
        /// {6} DateTime.UtcNow.Minute
        /// {7} DateTime.UtcNow.Second
        /// {8} DateTime.UtcNow.Millisecond
        /// {9} DateTime.UtcNow.Ticks
        /// </remarks>
        public void GenerateNewQueueName()
        {
            DateTime time = DateTime.UtcNow;
            Int32 testnumber = new Random().Next();
            String tableName = String.Format("{0}-{1}-{2}{3}{4}{5}{6}{7}{8}{9}", "TestQueue", testnumber, time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond, time.Ticks);
            this._queueName = tableName;
        }
        #endregion

    }
}
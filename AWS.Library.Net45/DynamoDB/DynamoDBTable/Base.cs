using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBTable
    /// </summary>
    public partial class DynamoDBTable
    {
        #region Fields
        /// <summary>
        /// Amazon DynamoDB client
        /// </summary>
        private AmazonDynamoDBClient _ddb;

        /// <summary>
        /// The name of the table.
        /// </summary>
        private String _tableName;

        /// <summary>
        /// The next write batch
        /// </summary>
        private DynamoDBBatchWrite _writeBatch;
        #endregion

        #region Properties
        /// <summary>
        /// The Name of the table
        /// </summary>
        public String Name
        {
            get
            {
                return _tableName;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a DynamoDBTable
        /// </summary>
        public DynamoDBTable(String accessKey, String secretAccessKey, String tableName)
        {
            _ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
            _tableName = tableName;
            _writeBatch = new DynamoDBBatchWrite(_tableName);
        }
        #endregion

        #region Methods

        #region BatchWrite
        /// <summary>
        /// Adds an item to be added in the next batch write.
        /// </summary>
        /// <param name="item">The item to be added in the next batch write</param>
        public void AddToBatchWritePut(DynamoDBItem item)
        {
            if (item.HasHashKey == false)
            {
                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
            }
            _writeBatch.Put(item);
        }

        /// <summary>
        /// Adds an item to be deleted in the next batch write.
        /// </summary>
        /// <param name="itemPrimaryKey">The items primary key.</param>
        public void AddToBatchWriteDelete(DynamoDBAttribute itemPrimaryKey)
        {
            if (itemPrimaryKey.IsHashKey == false)
            {
                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
            }
            _writeBatch.Delete(itemPrimaryKey);
        }
        #endregion

        /// <summary>
        /// Returns the name of the DynamoDBTable
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            return _tableName;
        }
        #endregion
    }
}
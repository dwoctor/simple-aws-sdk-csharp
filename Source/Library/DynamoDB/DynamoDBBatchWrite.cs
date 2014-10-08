using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;

namespace AWS.DynamoDB
{
    /// <summary>
    /// Stores the data for a batch write
    /// </summary>
    public class DynamoDBBatchWrite
    {
        #region Fields
        /// <summary>
        /// The name of the table.
        /// </summary>
        private String _tableName;

        /// <summary>
        /// The next write batch
        /// </summary>
        private List<Dictionary<String, List<WriteRequest>>> _writeBatch = new List<Dictionary<String, List<WriteRequest>>>();

        /// <summary>
        /// The number of requests in each sub-batch
        /// </summary>
        /// <remarks>
        /// Keeps track of the number of write requests in each sub-batch in the next write batch
        /// </remarks>
        private List<Int32> _writeBatchIndexRequestCounts = new List<Int32>();

        /// <summary>
        /// The index of the current sub-batch to add to
        /// </summary>
        /// <remarks>
        /// Keeps track of the sub-batch to add aditional requests to in the next write batch
        /// </remarks>
        private Int32 _writeBatchIndex = 0;
        #endregion

        #region Properties
        /// <summary>
        /// The Batch
        /// </summary>
        public List<Dictionary<String, List<WriteRequest>>> Batch
        {
            get
            {
				try
				{
                	return _writeBatch;
				}
				catch(Exception error)
				{
                    throw DynamoDBBatchWriteException.Generator(error);
				}
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a DynamoDBBatchWrite
        /// </summary>
        /// <param name="tableName">The name of the table the batch is to write to</param>
        public DynamoDBBatchWrite(String tableName)
        {
			try
			{
            	_tableName = tableName;
			}
			catch(Exception error)
			{
				throw DynamoDBBatchWriteException.Generator(error);
			}
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds an item to be added in the next batch write.
        /// </summary>
        /// <param name="item">The item to be added in the next batch write</param>
        public void Put(DynamoDBItem item)
        {
			try
			{
	            WriteRequest writeRequest = new WriteRequest();
	            PutRequest putRequest = new PutRequest();
	            putRequest.Item = item.ToDictionary();
	            writeRequest.PutRequest = putRequest;
	            Add(writeRequest);
			}
			catch(Exception error)
			{
				throw DynamoDBBatchWriteException.Generator(error);
			}
        }

        /// <summary>
        /// Adds an item to be deleted in the next batch write.
        /// </summary>
        /// <param name="itemPrimaryKey">The items primary key.</param>
        public void Delete(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
	            WriteRequest writeRequest = new WriteRequest();
	            DeleteRequest deleteRequest = new DeleteRequest();
	            deleteRequest.Key = itemPrimaryKey.Key;
	            writeRequest.DeleteRequest = deleteRequest;
	            Add(writeRequest);
			}
			catch(Exception error)
			{
				throw DynamoDBBatchWriteException.Generator(error);
			}
        }

        /// <summary>
        /// Adds an item to the next batch write.
        /// </summary>
        /// <param name="writeRequest">The write request</param>
        private void Add(WriteRequest writeRequest)
        {
			try
			{
	            try
	            {
	                _writeBatch[_writeBatchIndex][_tableName].Add(writeRequest);
	            }
	            catch (ArgumentOutOfRangeException)
	            {
	                _writeBatch.Add(new Dictionary<String, List<WriteRequest>>());
	                _writeBatch[_writeBatchIndex][_tableName] = new List<WriteRequest>() { writeRequest };
	                _writeBatchIndexRequestCounts.Add(0);
	            }
	            catch (KeyNotFoundException)
	            {
	                _writeBatch[_writeBatchIndex][_tableName] = new List<WriteRequest>() { writeRequest };
	            }
	            finally
	            {
	                _writeBatchIndexRequestCounts[_writeBatchIndex]++;
	                if (_writeBatchIndexRequestCounts[_writeBatchIndex] == 25)
	                {
	                    _writeBatchIndex++;
	                }
	            }
			}
			catch(Exception error)
			{
				throw DynamoDBBatchWriteException.Generator(error);
			}
        }

        /// <summary>
        /// Resets the DynamoDBBatchWrite
        /// </summary>
        public void Reset()
        {
			try
			{
	            _writeBatch = new List<Dictionary<String, List<WriteRequest>>>();
	            _writeBatchIndexRequestCounts = new List<Int32>();
	            _writeBatchIndex = 0;
			}
			catch(Exception error)
			{
				throw DynamoDBBatchWriteException.Generator(error);
			}
        }
        #endregion
    }
}
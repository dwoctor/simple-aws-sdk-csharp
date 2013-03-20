using Amazon;
using Amazon.DynamoDB;
using Amazon.DynamoDB.DataModel;
using Amazon.DynamoDB.DocumentModel;
using Amazon.DynamoDB.Model;
using Amazon.DynamoDB.Model.Internal;
using Amazon.DynamoDB.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDB Library
    /// </summary>
    public class DynamoDB
    {
        #region Enums
        /// <summary>
        /// Indicates an Update Action
        /// </summary>
        public enum UpdateAction { PUT, DELETE, ADD };

        /// <summary>
        /// Indicates the different Comparison Operators
        /// </summary>
        public enum ComparisonOperator { Equal, NotEqual, LessThanOrEqual, LessThan, GreaterThanOrEqual, GreaterThan, AttributeExists, AttributeDoesNotExist, ChecksForASubsequenceOrValueInASet, ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet, ChecksForAPrefix, ChecksForExactMatches, GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue };
        #endregion

        #region Fields
        /// <summary>
        /// Amazon DynamoDB client
        /// </summary>
        private AmazonDynamoDBClient _ddb;

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

        /// <summary>
        /// The strings of the different comparison operators
        /// </summary>
        private readonly Dictionary<ComparisonOperator, String> ComparisonOperatorString = new Dictionary<ComparisonOperator, String>()
        {
            {ComparisonOperator.Equal, "EQ"},
            {ComparisonOperator.NotEqual, "NE"},
            {ComparisonOperator.LessThanOrEqual, "LE"},
            {ComparisonOperator.LessThan, "LT"},
            {ComparisonOperator.GreaterThanOrEqual, "GE"},
            {ComparisonOperator.GreaterThan, "GT"},
            {ComparisonOperator.AttributeExists, "NOT_NULL"},
            {ComparisonOperator.AttributeDoesNotExist, "NULL"},
            {ComparisonOperator.ChecksForASubsequenceOrValueInASet, "CONTAINS"},
            {ComparisonOperator.ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet, "NOT_CONTAINS"},
            {ComparisonOperator.ChecksForAPrefix, "BEGINS_WITH"},
            {ComparisonOperator.ChecksForExactMatches, "IN"},
            {ComparisonOperator.GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue, "BETWEEN"}
        };
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a DynamoDB
        /// </summary>
        public DynamoDB(String accesskey, String secretAccessKey)
        {
            _ddb = new AmazonDynamoDBClient(accesskey, secretAccessKey);
        }
        #endregion

        #region Methods

        #region PutItem
        /// <summary>
        /// Puts an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to be put</param>
        /// <param name="attributeKey">The name of the item's attribute</param>
        /// <param name="attributeValue">The value of the item's attribute</param>
        public void PutItem(String tableName, String attributeKey, Object attributeValue)
        {
            PutItem(tableName, new Dictionary<String, Object>() { { attributeKey, attributeValue } });
        }

        /// <summary>
        /// Puts an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to be put</param>
        /// <param name="attributes">The item's attributes</param>
        public void PutItem(String tableName, Dictionary<String, Object> attributes)
        {
            Table table = Table.LoadTable(_ddb, tableName);
            Document document = new Document();
            foreach (var attribute in attributes)
            {
                if (attribute.Value is String) document[attribute.Key] =      (String)attribute.Value;
                else if (attribute.Value is Byte) document[attribute.Key] =   (Byte)attribute.Value;
                else if (attribute.Value is SByte) document[attribute.Key] =  (SByte)attribute.Value;
                else if (attribute.Value is Int16) document[attribute.Key] =  (Int16)attribute.Value;
                else if (attribute.Value is UInt16) document[attribute.Key] = (UInt16)attribute.Value;
                else if (attribute.Value is Int32) document[attribute.Key] =  (Int32)attribute.Value;
                else if (attribute.Value is UInt32) document[attribute.Key] = (UInt32)attribute.Value;
                else if (attribute.Value is Int64) document[attribute.Key] =  (Int64)attribute.Value;
                else if (attribute.Value is UInt64) document[attribute.Key] = (UInt64)attribute.Value;
                else if (attribute.Value is UInt64) document[attribute.Key] = (UInt64)attribute.Value;
                else if (attribute.Value is Guid) document[attribute.Key] =   new System.IO.MemoryStream(((Guid)attribute.Value).ToByteArray());
                else throw new Exception("Unsupported Type");
            }
            table.PutItem(document);
        }
        #endregion

        #region UpdateItem
        /// <summary>
        /// Updates an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to be updated</param>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributeKey">The name of the item's attribute to be updated</param>
        /// <param name="attributeValue">The value of the item's attribute to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public void UpdateItem(String tableName, Object itemPrimaryKey, String attributeKey, Object attributeValue, UpdateAction action)
        {
            UpdateItem(tableName, itemPrimaryKey, new Dictionary<String, Object>() { { attributeKey, attributeValue } }, action);
        }

        /// <summary>
        /// Updates an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to be updated</param>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributes">The item's attributes to be updated</param>
        /// <param name="action">The action to perform when updating the item's attributes</param>
        public void UpdateItem(String tableName, Object itemPrimaryKey, Dictionary<String, Object> attributes, UpdateAction action)
        {
            // Set Key
            UpdateItemRequest updateItemRequest = new UpdateItemRequest();
            updateItemRequest.TableName = tableName;
            updateItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemPrimaryKey) };
            // Set Attribute Value
            Dictionary<String, AttributeValueUpdate> itemAttributes = new Dictionary<String, AttributeValueUpdate>();
            foreach (var attribute in attributes)
            {
                AttributeValueUpdate attributeValueUpdate = new AttributeValueUpdate();
                attributeValueUpdate.Value = SetAttributeValue(attribute.Value);
                switch (action)
                {
                    case UpdateAction.PUT:
                        attributeValueUpdate.Action = "PUT";
                        break;
                    case UpdateAction.DELETE:
                        attributeValueUpdate.Action = "DELETE";
                        break;
                    case UpdateAction.ADD:
                        attributeValueUpdate.Action = "ADD";
                        break;
                }
                itemAttributes[attribute.Key] = attributeValueUpdate;
            }
            updateItemRequest.AttributeUpdates = itemAttributes;
            UpdateItemResponse putItemResponse = _ddb.UpdateItem(updateItemRequest);
        }
        #endregion

        #region BatchWrite
        /// <summary>
        /// Adds an item to be added in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to placed.</param>
        /// <param name="attributes">The items attributes.</param>
        public void AddToBatchWritePut(String tableName, Dictionary<String, Object> attributes)
        {
            AddToBatchWritePut(tableName, attributes, _writeBatch, _writeBatchIndexRequestCounts, _writeBatchIndex);
        }

        /// <summary>
        /// Adds an item to be added in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to placed.</param>
        /// <param name="attributes">The items attributes.</param>
        /// <param name="writeBatch">The write batch to add the item to</param>
        /// <param name="writeBatchIndexRequestCounts">The number of requests in each sub-batch</param>
        /// <param name="writeBatchIndex">The index of the current sub-batch to add to</param>
        private void AddToBatchWritePut(String tableName, Dictionary<String, Object> attributes, List<Dictionary<String, List<WriteRequest>>> writeBatch, List<Int32> writeBatchIndexRequestCounts, Int32 writeBatchIndex)
        {
            WriteRequest writeRequest = new WriteRequest();
            PutRequest putRequest = new PutRequest();
            Dictionary<String, AttributeValue> attributesFormated = new Dictionary<String, AttributeValue>();
            foreach (var attribute in attributes)
            {
                attributesFormated[attribute.Key] = SetAttributeValue(attribute.Value);
            }
            putRequest.Item = attributesFormated;
            writeRequest.PutRequest = putRequest;
            AddToBatchWrite(tableName, writeRequest, writeBatch, writeBatchIndexRequestCounts, writeBatchIndex);
        }

        /// <summary>
        /// Adds an item to the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to placed.</param>
        /// <param name="writeRequest">The write request</param>
        public void AddToBatchWrite(String tableName, WriteRequest writeRequest)
        {
            AddToBatchWrite(tableName, writeRequest, _writeBatch, _writeBatchIndexRequestCounts, _writeBatchIndex);
        }

        /// <summary>
        /// Adds an item to the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to placed.</param>
        /// <param name="writeRequest">The write request</param>
        /// <param name="writeBatch">The write batch to add the item to</param>
        /// <param name="writeBatchIndexRequestCounts">The number of requests in each sub-batch</param>
        /// <param name="writeBatchIndex">The index of the current sub-batch to add to</param>
        private void AddToBatchWrite(String tableName, WriteRequest writeRequest, List<Dictionary<String, List<WriteRequest>>> writeBatch, List<Int32> writeBatchIndexRequestCounts, Int32 writeBatchIndex)
        {
            try
            {
                writeBatch[writeBatchIndex][tableName].Add(writeRequest);
            }
            catch (ArgumentOutOfRangeException)
            {
                writeBatch.Add(new Dictionary<String, List<WriteRequest>>());
                writeBatch[writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
                writeBatchIndexRequestCounts.Add(0);
            }
            catch (KeyNotFoundException)
            {
                writeBatch[writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
            }
            finally
            {
                writeBatchIndexRequestCounts[writeBatchIndex]++;
                if (writeBatchIndexRequestCounts[writeBatchIndex] == 25)
                {
                    writeBatchIndex++;
                }
            }
        }

        /// <summary>
        /// Adds an item to be deleted in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to deleted.</param>
        /// <param name="itemPrimaryKey">The items primary key.</param>
        public void AddToBatchWriteDelete(String tableName, Object itemPrimaryKey)
        {
            AddToBatchWriteDelete(tableName, itemPrimaryKey, _writeBatch, _writeBatchIndexRequestCounts, _writeBatchIndex);
        }

        /// <summary>
        /// Adds an item to be deleted in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to deleted.</param>
        /// <param name="itemPrimaryKey">The items primary key.</param>
        /// <param name="writeBatch">The write batch to add the item to</param>
        /// <param name="writeBatchIndexRequestCounts">The number of requests in each sub-batch</param>
        /// <param name="writeBatchIndex">The index of the current sub-batch to add to</param>
        private void AddToBatchWriteDelete(String tableName, Object itemPrimaryKey, List<Dictionary<String, List<WriteRequest>>> writeBatch, List<Int32> writeBatchIndexRequestCounts, Int32 writeBatchIndex)
        {
            WriteRequest writeRequest = new WriteRequest();
            DeleteRequest deleteRequest = new DeleteRequest();
            deleteRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemPrimaryKey) };
            writeRequest.DeleteRequest = deleteRequest;
            AddToBatchWrite(tableName, writeRequest, writeBatch, writeBatchIndexRequestCounts, writeBatchIndex);
        }

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        public void BatchWriteExecute()
        {
            BatchWriteExecute(_writeBatch);
            _writeBatch = new List<Dictionary<String, List<WriteRequest>>>();
            _writeBatchIndexRequestCounts = new List<Int32>();
            _writeBatchIndex = 0;

        }

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        /// <param name="writeBatch"></param>
        public void BatchWriteExecute(List<Dictionary<String, List<WriteRequest>>> writeBatch)
        {
            foreach (var subBatch in writeBatch)
            {
                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
                batchWriteItemRequest.RequestItems = subBatch;
                BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
            }
        }

        /// <summary>
        /// Performs a batch write on items to be added
        /// </summary>
        /// <param name="data">Data to be added</param>
        /// <remarks>
        /// Data Format
        /// key = table name
        /// value = list of items containing their attributes
        /// </remarks>
        public void BatchWritePutExecute(Dictionary<String, List<Dictionary<String, Object>>> data)
        {
            List<Dictionary<String, List<WriteRequest>>> writeBatch = new List<Dictionary<String, List<WriteRequest>>>();
            List<Int32> writeBatchIndexRequestCounts = new List<Int32>();
            Int32 writeBatchIndex = 0;
            foreach (var table in data)
            {
                foreach (var item in table.Value)
                {
                    AddToBatchWritePut(table.Key, item, writeBatch, writeBatchIndexRequestCounts, writeBatchIndex);
                }
            }
            BatchWriteExecute(writeBatch);
        }

        /// <summary>
        /// Performs a batch write on items to be deleted
        /// </summary>
        /// <param name="data">Data to be deleted</param>
        /// <remarks>
        /// Data Format
        /// key = table name
        /// value = list of items containing their primary key
        /// </remarks>
        public void BatchWriteDeleteExecute(Dictionary<String, List<Object>> data)
        {
            List<Dictionary<String, List<WriteRequest>>> writeBatch = new List<Dictionary<String, List<WriteRequest>>>();
            List<Int32> writeBatchIndexRequestCounts = new List<Int32>();
            Int32 writeBatchIndex = 0;
            foreach (var table in data)
            {
                foreach (var item in table.Value)
                {
                    AddToBatchWriteDelete(table.Key, item, writeBatch, writeBatchIndexRequestCounts, writeBatchIndex);
                }
            }
            BatchWriteExecute(writeBatch);
        }
        #endregion

        #region DeleteItem
        /// <summary>
        /// Deletes an item from a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to be deleted</param>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public void DeleteItem(String tablename, Object itemPrimaryKey)
        {
            DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
            deleteItemRequest.TableName = tablename;
            deleteItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemPrimaryKey) };
            DeleteItemResponse deleteItemResponse = _ddb.DeleteItem(deleteItemRequest);
        }
        #endregion

        #region GetItem
        /// <summary>
        /// Gets an item from a table
        /// </summary>
        /// <param name="tableName">The name of the table where the item is located</param>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> GetItem(String tableName, Object itemPrimaryKey, List<String> attributes)
        {
            GetItemRequest getItemRequest = new GetItemRequest();
            getItemRequest.TableName = tableName;
            getItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemPrimaryKey) };
            getItemRequest.AttributesToGet = attributes;
            GetItemResponse getItemResponse = _ddb.GetItem(getItemRequest);
            GetItemResult getItemResult = getItemResponse.GetItemResult;
            Dictionary<String, Object> returnedAttributes = new Dictionary<String, Object>();
            foreach (var item in getItemResult.Item)
            {
                returnedAttributes[item.Key] = GetAttributeValue(item.Value);
            }
            return returnedAttributes;
        }
        #endregion

        #region ScanReturnItem
        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributeName, attributeValue, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributeName, attributeValue, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            scanRequest.ScanFilter = new Dictionary<String, Condition>() { { attributeName, new Condition() { AttributeValueList = new List<AttributeValue>() { SetAttributeValue(attributeValue) }, ComparisonOperator = ComparisonOperatorString[comparison] } } };
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItem(scanResult.Items);
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributeName, attributeValues, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributeName, attributeValues, comparison, new List<String>() { returnAttribute });
        }
        
        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            Condition condition = new Condition();
            filters[attributeName] = condition;
            condition.ComparisonOperator = ComparisonOperatorString[comparison];
            List<AttributeValue> attributeValueList = new List<AttributeValue>();
            foreach (var item in attributeValues)
            {
                attributeValueList.Add(SetAttributeValue(item));
            }
            condition.AttributeValueList = attributeValueList;
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItem(scanResult.Items);
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            foreach (var attribute in attributes)
            {
                Condition condition = new Condition();
                condition.AttributeValueList = new List<AttributeValue>() { SetAttributeValue(attribute.Value) };
                condition.ComparisonOperator = ComparisonOperatorString[comparison];
                filters[attribute.Key] = condition;
            }
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItem(scanResult.Items);
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            foreach (var attribute in attributes)
            {
                Condition condition = new Condition();
                List<AttributeValue> attributeValueList = new List<AttributeValue>();
                foreach (var value in attribute.Value)
                {
                    attributeValueList.Add(SetAttributeValue(value));
                }
                condition.AttributeValueList = attributeValueList;
                condition.ComparisonOperator = ComparisonOperatorString[comparison];
                filters[attribute.Key] = condition;
            }
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItem(scanResult.Items);
        }
        #endregion

        #region ScanReturnItems
        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributeName, attributeValue, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributeName, attributeValue, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValue">The value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            switch (comparison)
            {
                case ComparisonOperator.Equal:
                case ComparisonOperator.NotEqual:
                case ComparisonOperator.LessThanOrEqual:
                case ComparisonOperator.LessThan:
                case ComparisonOperator.GreaterThanOrEqual:
                case ComparisonOperator.GreaterThan:
                case ComparisonOperator.ChecksForASubsequenceOrValueInASet:
                case ComparisonOperator.ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet:
                case ComparisonOperator.ChecksForAPrefix:
                case ComparisonOperator.ChecksForExactMatches:
                case ComparisonOperator.GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue:
                    scanRequest.ScanFilter = new Dictionary<String, Condition>() { { attributeName, new Condition() { AttributeValueList = new List<AttributeValue>() { SetAttributeValue(attributeValue) }, ComparisonOperator = ComparisonOperatorString[comparison] } } };
                    break;
                case ComparisonOperator.AttributeExists:
                case ComparisonOperator.AttributeDoesNotExist:
                    scanRequest.ScanFilter = new Dictionary<String, Condition>() { { attributeName, new Condition() { ComparisonOperator = ComparisonOperatorString[comparison] } } };
                    break;
            }
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItems(scanResult.Items);
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributeName, attributeValues, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributeName, attributeValues, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributeName">The name of the attribute of the item to find</param>
        /// <param name="attributeValues">The values of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            Condition condition = new Condition();
            filters[attributeName] = condition;
            condition.ComparisonOperator = ComparisonOperatorString[comparison];
            List<AttributeValue> attributeValueList = new List<AttributeValue>();
            foreach (var item in attributeValues)
            {
                attributeValueList.Add(SetAttributeValue(item));
            }
            condition.AttributeValueList = attributeValueList;
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItems(scanResult.Items);
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attribute">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            foreach (var attribute in attributes)
            {
                Condition condition = new Condition();
                condition.AttributeValueList = new List<AttributeValue>() { SetAttributeValue(attribute.Value) };
                condition.ComparisonOperator = ComparisonOperatorString[comparison];
                filters[attribute.Key] = condition;
            }
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItems(scanResult.Items);
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="tableName">The name of the table to scan</param>
        /// <param name="attributes">The attributes of the item to find with mutiple values to compare</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filters = new Dictionary<String, Condition>();
            foreach (var attribute in attributes)
            {
                Condition condition = new Condition();
                List<AttributeValue> attributeValueList = new List<AttributeValue>();
                foreach (var value in attribute.Value)
                {
                    attributeValueList.Add(SetAttributeValue(value));
                }
                condition.AttributeValueList = attributeValueList;
                condition.ComparisonOperator = ComparisonOperatorString[comparison];
                filters[attribute.Key] = condition;
            }
            scanRequest.ScanFilter = filters;
            ScanResponse scanResponse = _ddb.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
            return ReturnedItems(scanResult.Items);
        }
        #endregion

        private Dictionary<String, Object> ReturnedItem(List<Dictionary<String, AttributeValue>> items)
        {
            Dictionary<String, Object> returnedAttributes = new Dictionary<String, Object>();
            if (items.Count == 1)
            {
                foreach (Dictionary<String, AttributeValue> item in items)
                {
                    foreach (var attribute in item)
                    {
                        returnedAttributes[attribute.Key] = GetAttributeValue(attribute.Value);
                    }
                }
            }
            return returnedAttributes;
        }

        private List<Dictionary<String, Object>> ReturnedItems(List<Dictionary<String, AttributeValue>> items)
        {
            List<Dictionary<String, Object>> returnedItems = new List<Dictionary<String, Object>>();
            foreach (Dictionary<String, AttributeValue> item in items)
            {
                Dictionary<String, Object> returnedAttributes = new Dictionary<String, Object>();
                foreach (var attribute in item)
                {
                    returnedAttributes[attribute.Key] = GetAttributeValue(attribute.Value);
                }
                returnedItems.Add(returnedAttributes);
            }
            return returnedItems;
        }

        private AttributeValue SetAttributeValue(Object value)
        {
            AttributeValue attributeValue = new AttributeValue();
            if (value is String)
                attributeValue.S = value.ToString();
            else if (value is Byte || value is SByte || value is Int16 || value is UInt16 || value is Int32 || value is UInt32 || value is Int64 || value is UInt64)
                attributeValue.N = value.ToString();
            else if (value is Byte[])
                attributeValue.B = new System.IO.MemoryStream(new Guid((Byte[])value).ToByteArray());
            else if (value is Guid)
                attributeValue.B = new System.IO.MemoryStream(((Guid)value).ToByteArray());
            else
                throw new Exception("Unsupported Type");
            return attributeValue;
        }

        private Object GetAttributeValue(AttributeValue attributeValue)
        {
            if (attributeValue.S != null)
                return attributeValue.S;
            else if (attributeValue.N != null)
            {
                try { return Byte.Parse(attributeValue.N); }
                catch { }
                try { return SByte.Parse(attributeValue.N); }
                catch { }
                try { return Int16.Parse(attributeValue.N); }
                catch { }
                try { return UInt16.Parse(attributeValue.N); }
                catch { }
                try { return Int32.Parse(attributeValue.N); }
                catch { }
                try { return UInt32.Parse(attributeValue.N); }
                catch { }
                try { return Int64.Parse(attributeValue.N); }
                catch { }
                try { return UInt64.Parse(attributeValue.N); }
                catch { }
                return null;
            }
            else if (attributeValue.B != null)
                return attributeValue.B.ToArray();
            else
                throw new Exception("Unsupported Type");
        }
        #endregion
    }
}
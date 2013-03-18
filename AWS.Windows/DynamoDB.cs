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

namespace AWS
{
    public class DynamoDB
    {
        #region Enums
        public enum Action { PUT, DELETE, ADD };

        public enum ComparisonOperator { Equal, NotEqual, LessThanOrEqual, LessThan, GreaterThanOrEqual, GreaterThan, AttributeExists, AttributeDoesNotExist, ChecksForASubsequenceOrValueInASet, ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet, ChecksForAPrefix, ChecksForExactMatches, GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue };
        #endregion

        #region Fields
        /// <summary>
        /// AmazonDynamoDB Client
        /// </summary>
        private AmazonDynamoDBClient _ddb;

        /// <summary>
        /// The Next Write Batch
        /// </summary>
        private List<Dictionary<String, List<WriteRequest>>> _writeBatch = new List<Dictionary<String, List<WriteRequest>>>();

        /// <summary>
        /// The Next Write Batch Index Request Counts
        /// </summary>
        /// <remarks>
        /// Keeps track of the number of write requests in each batch
        /// </remarks>
        private List<Int32> _writeBatchIndexRequestCounts = new List<Int32>();

        /// <summary>
        /// The Next Write Batch Index
        /// </summary>
        /// <remarks>
        /// Keeps track of the batch to add aditional requests too
        /// </remarks>
        private Int32 _writeBatchIndex = 0;

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
        /// Upon Construction of the Queue
        /// </summary>
        public DynamoDB(String accesskey, String secretAccessKey)
        {
            _ddb = new AmazonDynamoDBClient(accesskey, secretAccessKey);
            
        }
        #endregion

        #region Methods

        #region PutItem
        public void PutItem(String tableName, String attributeKey, Object attributeValue)
        {
            Table table = Table.LoadTable(_ddb, tableName);
            Document document = new Document();
            if (attributeValue is String)       document[attributeKey] = (String)attributeValue;
            else if (attributeValue is Byte)    document[attributeKey] = (Byte)attributeValue;
            else if (attributeValue is SByte)   document[attributeKey] = (SByte)attributeValue;
            else if (attributeValue is Int16)   document[attributeKey] = (Int16)attributeValue;
            else if (attributeValue is UInt16)  document[attributeKey] = (UInt16)attributeValue;
            else if (attributeValue is Int32)   document[attributeKey] = (Int32)attributeValue;
            else if (attributeValue is UInt32)  document[attributeKey] = (UInt32)attributeValue;
            else if (attributeValue is Int64)   document[attributeKey] = (Int64)attributeValue;
            else if (attributeValue is UInt64)  document[attributeKey] = (UInt64)attributeValue;
            else throw new Exception("Unsupported Type");
            table.PutItem(document);
        }
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
        public void UpdateItem(String tableName, Object itemKey, String attributeKey, Object attributeValue, Action action)
        {
            // Set Key
            UpdateItemRequest updateItemRequest = new UpdateItemRequest();
            updateItemRequest.TableName = tableName;
            updateItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemKey) };
            // Set Attribute Value
            Dictionary<String, AttributeValueUpdate> itemAttributes = new Dictionary<String, AttributeValueUpdate>();
            AttributeValueUpdate attributeValueUpdate = new AttributeValueUpdate();
            attributeValueUpdate.Value = SetAttributeValue(attributeValue);
            switch (action)
            {
                case Action.PUT:
                    attributeValueUpdate.Action = "PUT";
                    break;
                case Action.DELETE:
                    attributeValueUpdate.Action = "DELETE";
                    break;
                case Action.ADD:
                    attributeValueUpdate.Action = "ADD";
                    break;
            }
            itemAttributes[attributeKey] = attributeValueUpdate;
            updateItemRequest.AttributeUpdates = itemAttributes;
            UpdateItemResponse putItemResponse = _ddb.UpdateItem(updateItemRequest);
        }
        public void UpdateItem(String tableName, Object itemKey, Dictionary<String, Object> attributes, Action action)
        {
            // Set Key
            UpdateItemRequest updateItemRequest = new UpdateItemRequest();
            updateItemRequest.TableName = tableName;
            updateItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemKey) };
            // Set Attribute Value
            Dictionary<String, AttributeValueUpdate> itemAttributes = new Dictionary<String, AttributeValueUpdate>();
            foreach (var attribute in attributes)
            {
                AttributeValueUpdate attributeValueUpdate = new AttributeValueUpdate();
                attributeValueUpdate.Value = SetAttributeValue(attribute.Value);
                switch (action)
                {
                    case Action.PUT:
                        attributeValueUpdate.Action = "PUT";
                        break;
                    case Action.DELETE:
                        attributeValueUpdate.Action = "DELETE";
                        break;
                    case Action.ADD:
                        attributeValueUpdate.Action = "ADD";
                        break;
                }
                itemAttributes[attribute.Key] = attributeValueUpdate;
            }
            updateItemRequest.AttributeUpdates = itemAttributes;
            UpdateItemResponse putItemResponse = _ddb.UpdateItem(updateItemRequest);
        }
        #endregion

        #region WriteBatch

        /// <summary>
        /// Adds an item to be Deleted in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to deleted.</param>
        /// <param name="attributes">The items primary key.</param>
        public void AddToBatchWriteDelete(String tableName, Object itemKey)
        {
            WriteRequest writeRequest = new WriteRequest();
            DeleteRequest deleteRequest = new DeleteRequest();
            deleteRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemKey) };
            writeRequest.DeleteRequest = deleteRequest;
            try
            {
                _writeBatch[_writeBatchIndex][tableName].Add(writeRequest);
            }
            catch (ArgumentOutOfRangeException)
            {
                _writeBatch.Add(new Dictionary<String, List<WriteRequest>>());
                _writeBatch[_writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
                _writeBatchIndexRequestCounts.Add(0);
            }
            catch (KeyNotFoundException)
            {
                _writeBatch[_writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
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

        /// <summary>
        /// Adds an item to be added in the next batch write.
        /// </summary>
        /// <param name="tableName">The name of the table where the item is to placed.</param>
        /// <param name="attributes">The items attributes.</param>
        public void AddToBatchWritePut(String tableName, Dictionary<String, Object> attributes)
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
            try
            {
                _writeBatch[_writeBatchIndex][tableName].Add(writeRequest);
            }
            catch (ArgumentOutOfRangeException)
            {
                _writeBatch.Add(new Dictionary<String, List<WriteRequest>>());
                _writeBatch[_writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
                _writeBatchIndexRequestCounts.Add(0);
            }
            catch (KeyNotFoundException)
            {
                _writeBatch[_writeBatchIndex][tableName] = new List<WriteRequest>() { writeRequest };
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

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        public void BatchWriteExecute()
        {
            foreach (var subBatch in _writeBatch)
            {
                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
                batchWriteItemRequest.RequestItems = subBatch;
                BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
            }
            _writeBatch = new List<Dictionary<String, List<WriteRequest>>>();
            _writeBatchIndexRequestCounts = new List<Int32>();
            _writeBatchIndex = 0;

        }

        /// <summary>
        /// Performs a batch write on items to be added
        /// </summary>
        /// <param name="batch">Data to be added</param>
        /// <remarks>
        /// Data Format
        /// key = table name
        /// value = list of items containing their attributes
        /// </remarks>
        public void BatchWritePutExecute(Dictionary<String, List<Dictionary<String, Object>>> batch)
        {
            BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
            Dictionary<String, List<WriteRequest>> requests = new Dictionary<String, List<WriteRequest>>();
            foreach (var table in batch)
            {
                List<WriteRequest> writeRequests = new List<WriteRequest>();
                foreach (var item in table.Value)
                {
                    WriteRequest writeRequest = new WriteRequest();
                    PutRequest putRequest = new PutRequest();
                    Dictionary<String, AttributeValue> attributes = new Dictionary<String, AttributeValue>();
                    foreach (var attribute in item)
                    {
                        attributes[attribute.Key] = SetAttributeValue(attribute.Value);
                    }
                    putRequest.Item = attributes;
                    writeRequest.PutRequest = putRequest;
                    writeRequests.Add(writeRequest);
                }
                requests[table.Key] = writeRequests;
            }
            batchWriteItemRequest.RequestItems = requests;
            BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
        }

        /// <summary>
        /// Performs a batch write on items to be deleted
        /// </summary>
        /// <param name="batch">Data to be deleted</param>
        /// <remarks>
        /// Data Format
        /// key = table name
        /// value = list of items containing their primary key
        /// </remarks>
        public void BatchWriteDeleteExecute(Dictionary<String, List<Object>> batch)
        {
            BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
            Dictionary<String, List<WriteRequest>> requests = new Dictionary<String, List<WriteRequest>>();
            foreach (var table in batch)
            {
                List<WriteRequest> writeRequests = new List<WriteRequest>();
                foreach (var item in table.Value)
                {
                    WriteRequest writeRequest = new WriteRequest();
                    DeleteRequest deleteRequest = new DeleteRequest();
                    deleteRequest.Key = new Key() { HashKeyElement = SetAttributeValue(item) };
                    writeRequest.DeleteRequest = deleteRequest;
                    writeRequests.Add(writeRequest);
                }
                requests[table.Key] = writeRequests;
            }
            batchWriteItemRequest.RequestItems = requests;
            BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
        }

        #endregion

        public void DeleteItem(String tablename, Object itemKey)
        {
            DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
            deleteItemRequest.TableName = tablename;
            deleteItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemKey) };
            DeleteItemResponse deleteItemResponse = _ddb.DeleteItem(deleteItemRequest);
        }

        public Dictionary<String, Object> GetItem(String tableName, Object itemKey, List<String> attributes)
        {
            GetItemRequest getItemRequest = new GetItemRequest();
            getItemRequest.TableName = tableName;
            getItemRequest.Key = new Key() { HashKeyElement = SetAttributeValue(itemKey) };
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

        #region ScanReturnItem
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributeName, attributeValue, comparison, new List<String>());
        }
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributeName, attributeValue, comparison, new List<String>() { returnAttribute });
        }
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
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributeName, attributeValues, comparison, new List<String>());
        }
        public Dictionary<String, Object> ScanReturnItem(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributeName, attributeValues, comparison, new List<String>() { returnAttribute });
        }
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
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>());
        }
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }
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
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>());
        }
        public Dictionary<String, Object> ScanReturnItem(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItem(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }
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
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributeName, attributeValue, comparison, new List<String>());
        }
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, Object attributeValue, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributeName, attributeValue, comparison, new List<String>() { returnAttribute });
        }
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
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributeName, attributeValues, comparison, new List<String>());
        }
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, String attributeName, List<Object> attributeValues, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributeName, attributeValues, comparison, new List<String>() { returnAttribute });
        }
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
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>());
        }
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, Object> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }
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
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>());
        }
        public List<Dictionary<String, Object>> ScanReturnItems(String tableName, Dictionary<String, List<Object>> attributes, ComparisonOperator comparison, String returnAttribute)
        {
            return ScanReturnItems(tableName, attributes, comparison, new List<String>() { returnAttribute });
        }
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
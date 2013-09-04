//using Amazon.DynamoDBv2;
////using Amazon.DynamoDBv2.DataModel;
////using Amazon.DynamoDBv2.DocumentModel;
//using Amazon.DynamoDBv2.Model;
////using Amazon.DynamoDBv2.Model.Internal;
////using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
//using AWS.DynamoDB;
//using AWS.DynamoDB.Exceptions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace AWS.DynamoDB
//{
//    /// <summary>
//    /// A DynamoDBTable
//    /// </summary>
//    public class DynamoDBTableold
//    {
//        #region Fields
//        /// <summary>
//        /// Amazon DynamoDB client
//        /// </summary>
//        private AmazonDynamoDBClient _ddb;

//        /// <summary>
//        /// The name of the table.
//        /// </summary>
//        private String _tableName;

//        /// <summary>
//        /// The next write batch
//        /// </summary>
//        private DynamoDBBatchWrite _writeBatch;
//        #endregion

//        #region Properties
//        /// <summary>
//        /// The Name of the table
//        /// </summary>
//        public String Name
//        {
//            get
//            {
//                return _tableName;
//            }
//        }
//        #endregion

//        #region Constructors
//        /// <summary>
//        /// Constructs a DynamoDBTable
//        /// </summary>
//        public DynamoDBTableold(String accessKey, String secretAccessKey, String tableName)
//        {
//            _ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
//            _tableName = tableName;
//            _writeBatch = new DynamoDBBatchWrite(_tableName);
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Creates a table associated with an AWS account
//        /// </summary>
//        /// <param name="accessKey">The Access Key of the AWS account</param>
//        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
//        /// <param name="tableName">The name of the table to create</param>
//        /// <param name="hashKeyAttributeName">The name of the hash key</param>
//        /// <param name="hashKeyAttributeType">The type of the hash key</param>
//        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
//        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
//        public static DynamoDBTable CreateTable(String accessKey, String secretAccessKey, String tableName, String hashKeyAttributeName, Types.Enum hashKeyAttributeType, Int64 readCapacityUnits, Int64 writeCapacityUnits)
//        {
//            if (GetDictionaryOfTables(accessKey, secretAccessKey).ContainsKey(tableName) == true)
//            {
//                throw DynamoDBTableExceptions.TableAlreadyExists(tableName);
//            }
//            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
//            KeySchemaElement keySchemaElement = new KeySchemaElement() { AttributeName = hashKeyAttributeName, KeyType = Types.KeyType[hashKeyAttributeType]  };
//            List<KeySchemaElement> keySchema = new List<KeySchemaElement>() { keySchemaElement };
//            ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput() { ReadCapacityUnits = readCapacityUnits, WriteCapacityUnits = writeCapacityUnits };
//            CreateTableRequest createTableRequest = new CreateTableRequest() { TableName = tableName, KeySchema = keySchema, ProvisionedThroughput = provisionedThroughput };
//            CreateTableResponse createTableResponse = ddb.CreateTable(createTableRequest);
//            return new DynamoDBTable(accessKey, secretAccessKey, tableName);
//        }

//        /// <summary>
//        /// Delete a table associated with an AWS account
//        /// </summary>
//        /// <param name="accessKey">The Access Key of the AWS account</param>
//        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
//        /// <param name="tableName">The name of the table to delete</param>
//        public static void DeleteTable(String accessKey, String secretAccessKey, String tableName)
//        {
//            if (GetDictionaryOfTables(accessKey, secretAccessKey).ContainsKey(tableName) == false)
//            {
//                throw DynamoDBTableExceptions.TableDoesNotExist(tableName);
//            }
//            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
//            DeleteTableRequest deleteTableRequest = new DeleteTableRequest() { TableName = tableName };
//            DeleteTableResponse deleteTableResponse = ddb.DeleteTable(deleteTableRequest);
//        }

//        /// <summary>
//        /// Gets a list of tables associated with an AWS account
//        /// </summary>
//        /// <param name="accessKey">The Access Key of the AWS account</param>
//        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
//        public static List<DynamoDBTable> GetListOfTables(String accessKey, String secretAccessKey)
//        {
//            List<DynamoDBTable> tables = new List<DynamoDBTable>();
//            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
//            ListTablesRequest listTablesRequest = new ListTablesRequest();
//            ListTablesResponse listTablesResponse = ddb.ListTables(listTablesRequest);
//            ListTablesResult listTablesResult = listTablesResponse.ListTablesResult;
//            foreach (var item in listTablesResult.TableNames)
//            {
//                tables.Add(new DynamoDBTable(accessKey, secretAccessKey, item));
//            }
//            return tables;
//        }

//        /// <summary>
//        /// Gets a dictionary of tables associated with an AWS account
//        /// </summary>
//        /// <param name="accessKey">The Access Key of the AWS account</param>
//        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
//        public static Dictionary<String, DynamoDBTable> GetDictionaryOfTables(String accessKey, String secretAccessKey)
//        {
//            Dictionary<String, DynamoDBTable> tables = new Dictionary<String, DynamoDBTable>();
//            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
//            ListTablesRequest listTablesRequest = new ListTablesRequest();
//            ListTablesResponse listTablesResponse = ddb.ListTables(listTablesRequest);
//            ListTablesResult listTablesResult = listTablesResponse.ListTablesResult;
//            foreach (var item in listTablesResult.TableNames)
//            {
//                tables.Add(item, new DynamoDBTable(accessKey, secretAccessKey, item));
//            }
//            return tables;
//        }

//        #region PutItem
//        /// <summary>
//        /// Puts an item in the table
//        /// </summary>
//        /// <param name="item">The item to put in the table</param>
//        public void PutItem(DynamoDBItem item)
//        {
//            if (item.HasHashKey)
//            {
//                PutItemRequest putItemRequest = new PutItemRequest();
//                putItemRequest.TableName = _tableName;
//                putItemRequest.Item = item.ToDictionary();
//                _ddb.PutItem(putItemRequest);
//            }
//            else
//            {
//                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
//            }
//        }
//        #endregion

//        #region UpdateItem
//        /// <summary>
//        /// Updates an item in the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// <param name="attribute">The item's attribute to be updated</param>
//        /// <param name="action">The action to perform when updating the item's attribute</param>
//        public void UpdateItem(DynamoDBAttribute itemPrimaryKey, DynamoDBAttribute attribute, UpdateAction action)
//        {
//            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
//            item.Add(attribute);
//            UpdateItem(item, action);
//        }

//        /// <summary>
//        /// Updates an item in the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// <param name="attributes">The item's attributes to be updated</param>
//        /// <param name="action">The action to perform when updating the item's attributes</param>
//        public void UpdateItem(DynamoDBAttribute itemPrimaryKey, List<DynamoDBAttribute> attributes, UpdateAction action)
//        {
//            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
//            item.Add(attributes);
//            UpdateItem(item, action);
//        }

//        /// <summary>
//        /// Updates an item in the table
//        /// </summary>
//        /// <param name="item">The item to be updated</param>
//        /// <param name="action">The action to perform when updating the item's attribute</param>
//        public void UpdateItem(DynamoDBItem item, UpdateAction action)
//        {
//            if (item.HasHashKey == false)
//            {
//                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
//            }
//            // Set Key
//            UpdateItemRequest updateItemRequest = new UpdateItemRequest();
//            updateItemRequest.TableName = _tableName;
//            updateItemRequest.Key = item.HashKey.Key;
//            // Set Attribute Value
//            Dictionary<String, AttributeValueUpdate> itemAttributes = new Dictionary<String, AttributeValueUpdate>();
//            foreach (var attribute in item)
//            {
//                if (attribute.IsHashKey == false)
//                {
//                    switch (action)
//                    {
//                        case UpdateAction.PUT:
//                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "PUT", Value = attribute.ToAttributeValue() };
//                            break;
//                        case UpdateAction.DELETE:
//                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "DELETE" };
//                            break;
//                        case UpdateAction.ADD:
//                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "ADD", Value = attribute.ToAttributeValue() };
//                            break;
//                    }
//                }
//            }
//            updateItemRequest.AttributeUpdates = itemAttributes;
//            UpdateItemResponse putItemResponse = _ddb.UpdateItem(updateItemRequest);
//        }
//        #endregion

//        #region BatchWrite
//        /// <summary>
//        /// Adds an item to be added in the next batch write.
//        /// </summary>
//        /// <param name="item">The item to be added in the next batch write</param>
//        public void AddToBatchWritePut(DynamoDBItem item)
//        {
//            if (item.HasHashKey == false)
//            {
//                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
//            }
//            _writeBatch.Put(item);
//        }

//        /// <summary>
//        /// Adds an item to be deleted in the next batch write.
//        /// </summary>
//        /// <param name="itemPrimaryKey">The items primary key.</param>
//        public void AddToBatchWriteDelete(DynamoDBAttribute itemPrimaryKey)
//        {
//            if (itemPrimaryKey.IsHashKey == false)
//            {
//                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
//            }
//            _writeBatch.Delete(itemPrimaryKey);
//        }

//        /// <summary>
//        /// Performs a batch write on items already added to the batch
//        /// </summary>
//        public void BatchWriteExecute()
//        {
//            BatchWriteExecute(_writeBatch);
//            _writeBatch.Reset();
//        }

//        /// <summary>
//        /// Performs a batch write on items already added to the batch
//        /// </summary>
//        private void BatchWriteExecute(DynamoDBBatchWrite writeBatch)
//        {
//            foreach (var subBatch in writeBatch.Batch)
//            {
//                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
//                batchWriteItemRequest.RequestItems = subBatch;
//                BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
//            }
//        }

//        /// <summary>
//        /// Performs a batch write on items to be added
//        /// </summary>
//        /// <param name="data">List of items containing their attributes to be added</param>
//        public void BatchWritePutExecute(List<DynamoDBItem> data)
//        {
//            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
//            foreach (var item in data)
//            {
//                if (item.HasHashKey == false)
//                {
//                    throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
//                }
//                writeBatch.Put(item);
//            }
//            BatchWriteExecute(writeBatch);
//        }

//        /// <summary>
//        /// Performs a batch write on items to be deleted
//        /// </summary>
//        /// <param name="data">List of items containing their primary key to be deleted</param>
//        public void BatchWriteDeleteExecute(List<DynamoDBAttribute> data)
//        {
//            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
//            foreach (var primaryKey in data)
//            {
//                if (primaryKey.IsHashKey == false)
//                {
//                    throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
//                }
//                writeBatch.Delete(primaryKey);
//            }
//            BatchWriteExecute(writeBatch);
//        }
//        #endregion

//        #region DeleteItem
//        /// <summary>
//        /// Deletes an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
//        {
//            if (itemPrimaryKey.IsHashKey)
//            {
//                DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
//                deleteItemRequest.TableName = _tableName;
//                deleteItemRequest.Key = itemPrimaryKey.Key;
//                DeleteItemResponse deleteItemResponse = _ddb.DeleteItem(deleteItemRequest);
//            }
//            else
//            {
//                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
//            }
//        }
//        #endregion

//        #region GetItem
//        /// <summary>
//        /// Gets an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey)
//        {
//            return GetItem(itemPrimaryKey, new List<String>());
//        }

//        /// <summary>
//        /// Gets an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, String returnAttribute)
//        {
//            return GetItem(itemPrimaryKey, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Gets an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, IEnumerable<String> returnAttributes)
//        {
//            return GetItem(itemPrimaryKey, returnAttributes.ToList());
//        }

//        /// <summary>
//        /// Gets an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, List<String> returnAttributes)
//        {
//            if (itemPrimaryKey.IsHashKey == false)
//            {
//                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
//            }
//            GetItemRequest getItemRequest = new GetItemRequest();
//            getItemRequest.TableName = _tableName;
//            getItemRequest.Key = itemPrimaryKey.Key;
//            getItemRequest.AttributesToGet = returnAttributes;
//            GetItemResponse getItemResponse = _ddb.GetItem(getItemRequest);
//            GetItemResult getItemResult = getItemResponse.GetItemResult;
//            return new DynamoDBItem(getItemResult.Item);
//        }
//        #endregion

//        #region ScanReturnItem
//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItem(new DynamoDBItem(attribute), comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItem(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            return ScanReturnItem(new DynamoDBItem(attribute), comparison, returnAttributes);
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItem(new DynamoDBItem(attributes), comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItem(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            return ScanReturnItem(new DynamoDBItem(attributes), comparison, returnAttributes);
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItem(item, comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItem(item, comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>An item's attributes</returns>
//        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            ScanResult scanResult = Scan(item, comparison, returnAttributes);
//            return new DynamoDBItem(scanResult.Items);
//        }
//        #endregion

//        #region ScanReturnItems
//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItems(new DynamoDBItem(attribute), comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItems(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attribute">The name and value of the attribute of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            return ScanReturnItems(new DynamoDBItem(attribute), comparison, returnAttributes);
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItems(new DynamoDBItem(attributes), comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItems(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="attributes">The attributes of the item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            return ScanReturnItems(new DynamoDBItem(attributes), comparison, returnAttributes);
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison)
//        {
//            return ScanReturnItems(item, comparison, new List<String>());
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
//        {
//            return ScanReturnItems(item, comparison, new List<String>() { returnAttribute });
//        }

//        /// <summary>
//        /// Scans for an items in a table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
//        /// <returns>Multiple items with their attributes</returns>
//        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            ScanResult scanResult = Scan(item, comparison, returnAttributes);
//            List<DynamoDBItem> returnItems = new List<DynamoDBItem>();
//            foreach (var scanResultItem in scanResult.Items)
//            {
//                returnItems.Add(new DynamoDBItem(scanResultItem));
//            }
//            return returnItems;
//        }
//        #endregion

//        /// <summary>
//        /// Scans for an item in the table
//        /// </summary>
//        /// <param name="item">The item to find</param>
//        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
//        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
//        /// <returns>The Scan Result</returns>
//        private ScanResult Scan(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
//        {
//            ScanRequest scanRequest = new ScanRequest();
//            scanRequest.TableName = _tableName;
//            scanRequest.AttributesToGet = returnAttributes;
//            Dictionary<String, Condition> filter = new Dictionary<String, Condition>();
//            var groupedAttributes = item.GetAttributesGroupedByName();
//            foreach (var attributeGroup in groupedAttributes)
//            {
//                Condition condition = new Condition();
//                switch (comparison)
//                {
//                    case ComparisonOperator.Enum.Equal:
//                    case ComparisonOperator.Enum.NotEqual:
//                    case ComparisonOperator.Enum.LessThanOrEqual:
//                    case ComparisonOperator.Enum.LessThan:
//                    case ComparisonOperator.Enum.GreaterThanOrEqual:
//                    case ComparisonOperator.Enum.GreaterThan:
//                    case ComparisonOperator.Enum.ChecksForASubsequenceOrValueInASet:
//                    case ComparisonOperator.Enum.ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet:
//                    case ComparisonOperator.Enum.ChecksForAPrefix:
//                    case ComparisonOperator.Enum.ChecksForExactMatches:
//                    case ComparisonOperator.Enum.GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue:
//                        List<AttributeValue> attributeValueList = new List<AttributeValue>();
//                        foreach (var value in attributeGroup.Value)
//                        {
//                            attributeValueList.Add(value.ToAttributeValue());
//                        }
//                        condition.AttributeValueList = attributeValueList;
//                        break;
//                }
//                condition.ComparisonOperator = ComparisonOperator.String[comparison];
//                filter[attributeGroup.Key] = condition;
//            }
//            scanRequest.ScanFilter = filter;
//            ScanResponse scanResponse = _ddb.Scan(scanRequest);
//            ScanResult scanResult = scanResponse.ScanResult;
//            return scanResult;
//        }

//        /// <summary>
//        /// Returns the name of the DynamoDBTable
//        /// </summary>
//        /// <returns>String</returns>
//        public override String ToString()
//        {
//            return _tableName;
//        }
//        #endregion
//    }
//}
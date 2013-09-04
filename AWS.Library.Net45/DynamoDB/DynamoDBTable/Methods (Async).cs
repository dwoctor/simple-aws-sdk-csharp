﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.DynamoDB
{
    public partial class DynamoDBTable
    {
        #region Methods
        /// <summary>
        /// Creates a table associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="hashKeyAttributeName">The name of the hash key</param>
        /// <param name="hashKeyAttributeType">The type of the hash key</param>
        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
        public static async Task<DynamoDBTable> CreateTableAsync(String accessKey, String secretAccessKey, String tableName, String hashKeyAttributeName, Types.Enum hashKeyAttributeType, Int64 readCapacityUnits, Int64 writeCapacityUnits)
        {
            Dictionary<String, DynamoDBTable> tables = await GetDictionaryOfTablesAsync(accessKey, secretAccessKey);
            if (tables.ContainsKey(tableName) == true)
            {
                throw DynamoDBTableExceptions.TableAlreadyExists(tableName);
            }
            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
            KeySchemaElement keySchemaElement = new KeySchemaElement() { AttributeName = hashKeyAttributeName, KeyType = Types.KeyType[hashKeyAttributeType] };
            List<KeySchemaElement> keySchema = new List<KeySchemaElement>() { keySchemaElement };
            ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput() { ReadCapacityUnits = readCapacityUnits, WriteCapacityUnits = writeCapacityUnits };
            CreateTableRequest createTableRequest = new CreateTableRequest() { TableName = tableName, KeySchema = keySchema, ProvisionedThroughput = provisionedThroughput };
            CreateTableResponse createTableResponse = await ddb.CreateTableAsync(createTableRequest);
            return new DynamoDBTable(accessKey, secretAccessKey, tableName);
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="tableName">The name of the table to delete</param>
        public static async void DeleteTableAsync(String accessKey, String secretAccessKey, String tableName)
        {
            Dictionary<String, DynamoDBTable> tables = await GetDictionaryOfTablesAsync(accessKey, secretAccessKey);
            if (tables.ContainsKey(tableName) == false)
            {
                throw DynamoDBTableExceptions.TableDoesNotExist(tableName);
            }
            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
            DeleteTableRequest deleteTableRequest = new DeleteTableRequest() { TableName = tableName };
            DeleteTableResponse deleteTableResponse = await ddb.DeleteTableAsync(deleteTableRequest);
        }

        /// <summary>
        /// Gets a list of tables associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static async Task<List<DynamoDBTable>> GetListOfTablesAsync(String accessKey, String secretAccessKey)
        {
            List<DynamoDBTable> tables = new List<DynamoDBTable>();
            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
            ListTablesRequest listTablesRequest = new ListTablesRequest();
            ListTablesResponse listTablesResponse = await ddb.ListTablesAsync(listTablesRequest);
            foreach (var item in listTablesResponse.TableNames)
            {
                tables.Add(new DynamoDBTable(accessKey, secretAccessKey, item));
            }
            return tables;
        }

        /// <summary>
        /// Gets a dictionary of tables associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static async Task<Dictionary<String, DynamoDBTable>> GetDictionaryOfTablesAsync(String accessKey, String secretAccessKey)
        {
            Dictionary<String, DynamoDBTable> tables = new Dictionary<String, DynamoDBTable>();
            AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(accessKey, secretAccessKey);
            ListTablesRequest listTablesRequest = new ListTablesRequest();
            ListTablesResponse listTablesResponse = await ddb.ListTablesAsync(listTablesRequest);
            foreach (var item in listTablesResponse.TableNames)
            {
                tables.Add(item, new DynamoDBTable(accessKey, secretAccessKey, item));
            }
            return tables;
        }

        #region PutItemAsync
        /// <summary>
        /// Puts an item in the table
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public async void PutItemAsync(DynamoDBItem item)
        {
            if (item.HasHashKey)
            {
                PutItemRequest putItemRequest = new PutItemRequest();
                putItemRequest.TableName = _tableName;
                putItemRequest.Item = item.ToDictionary();
                await _ddb.PutItemAsync(putItemRequest);
            }
            else
            {
                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
            }
        }
        #endregion

        #region UpdateItemAsync
        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attribute">The item's attribute to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public void UpdateItemAsync(DynamoDBAttribute itemPrimaryKey, DynamoDBAttribute attribute, UpdateAction action)
        {
            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
            item.Add(attribute);
            UpdateItemAsync(item, action);
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributes">The item's attributes to be updated</param>
        /// <param name="action">The action to perform when updating the item's attributes</param>
        public void UpdateItemAsync(DynamoDBAttribute itemPrimaryKey, List<DynamoDBAttribute> attributes, UpdateAction action)
        {
            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
            item.Add(attributes);
            UpdateItemAsync(item, action);
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="item">The item to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public async void UpdateItemAsync(DynamoDBItem item, UpdateAction action)
        {
            if (item.HasHashKey == false)
            {
                throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
            }
            // Set Key
            UpdateItemRequest updateItemRequest = new UpdateItemRequest();
            updateItemRequest.TableName = _tableName;
            updateItemRequest.Key = item.HashKey.Key;
            // Set Attribute Value
            Dictionary<String, AttributeValueUpdate> itemAttributes = new Dictionary<String, AttributeValueUpdate>();
            foreach (var attribute in item)
            {
                if (attribute.IsHashKey == false)
                {
                    switch (action)
                    {
                        case UpdateAction.PUT:
                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "PUT", Value = attribute.ToAttributeValue() };
                            break;
                        case UpdateAction.DELETE:
                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "DELETE" };
                            break;
                        case UpdateAction.ADD:
                            itemAttributes[attribute.Name] = new AttributeValueUpdate() { Action = "ADD", Value = attribute.ToAttributeValue() };
                            break;
                    }
                }
            }
            updateItemRequest.AttributeUpdates = itemAttributes;
            UpdateItemResponse putItemResponse = await _ddb.UpdateItemAsync(updateItemRequest);
        }
        #endregion

        #region BatchWriteAsync
        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        public void BatchWriteExecuteAsync()
        {
            BatchWriteExecuteAsync(_writeBatch);
            _writeBatch.Reset();
        }

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        private async void BatchWriteExecuteAsync(DynamoDBBatchWrite writeBatch)
        {
            foreach (var subBatch in writeBatch.Batch)
            {
                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
                batchWriteItemRequest.RequestItems = subBatch;
                BatchWriteItemResponse batchWriteItemResponse = await _ddb.BatchWriteItemAsync(batchWriteItemRequest);
            }
        }

        /// <summary>
        /// Performs a batch write on items to be added
        /// </summary>
        /// <param name="data">List of items containing their attributes to be added</param>
        public void BatchWritePutExecuteAsync(List<DynamoDBItem> data)
        {
            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
            foreach (var item in data)
            {
                if (item.HasHashKey == false)
                {
                    throw DynamoDBTableExceptions.DynamoDBItemContainsNoHashKey();
                }
                writeBatch.Put(item);
            }
            BatchWriteExecuteAsync(writeBatch);
        }

        /// <summary>
        /// Performs a batch write on items to be deleted
        /// </summary>
        /// <param name="data">List of items containing their primary key to be deleted</param>
        public void BatchWriteDeleteExecuteAsync(List<DynamoDBAttribute> data)
        {
            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
            foreach (var primaryKey in data)
            {
                if (primaryKey.IsHashKey == false)
                {
                    throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
                }
                writeBatch.Delete(primaryKey);
            }
            BatchWriteExecuteAsync(writeBatch);
        }
        #endregion

        #region DeleteItemAsync
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public async void DeleteItemAsync(DynamoDBAttribute itemPrimaryKey)
        {
            if (itemPrimaryKey.IsHashKey)
            {
                DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
                deleteItemRequest.TableName = _tableName;
                deleteItemRequest.Key = itemPrimaryKey.Key;
                DeleteItemResponse deleteItemResponse = await _ddb.DeleteItemAsync(deleteItemRequest);
            }
            else
            {
                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
            }
        }
        #endregion

        #region GetItemAsync
        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey)
        {
            return await GetItemAsync(itemPrimaryKey, new List<String>());
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, String returnAttribute)
        {
            return await GetItemAsync(itemPrimaryKey, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, IEnumerable<String> returnAttributes)
        {
            return await GetItemAsync(itemPrimaryKey, returnAttributes.ToList());
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, List<String> returnAttributes)
        {
            if (itemPrimaryKey.IsHashKey == false)
            {
                throw DynamoDBTableExceptions.DynamoDBAttributeContainsNoHashKey();
            }
            GetItemRequest getItemRequest = new GetItemRequest();
            getItemRequest.TableName = _tableName;
            getItemRequest.Key = itemPrimaryKey.Key;
            getItemRequest.AttributesToGet = returnAttributes;
            GetItemResponse getItemResponse = await _ddb.GetItemAsync(getItemRequest);
            return new DynamoDBItem(getItemResponse.Item);
        }
        #endregion

        #region ScanReturnItemAsync
        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, returnAttributes);
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, returnAttributes);
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemAsync(item, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemAsync(item, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            List<Dictionary<String, AttributeValue>> scanResult = await ScanAsync(item, comparison, returnAttributes);
            return new DynamoDBItem(scanResult);
        }
        #endregion

        #region ScanReturnItemsAsync
        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, returnAttributes);
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, returnAttributes);
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
            return await ScanReturnItemsAsync(item, comparison, new List<String>());
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
        {
            return await ScanReturnItemsAsync(item, comparison, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            List<Dictionary<String, AttributeValue>> scanResult = await ScanAsync(item, comparison, returnAttributes);
            List<DynamoDBItem> returnItems = new List<DynamoDBItem>();
            foreach (var scanResultItem in scanResult)
            {
                returnItems.Add(new DynamoDBItem(scanResultItem));
            }
            return returnItems;
        }
        #endregion

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>The Scan Result</returns>
        private async Task<List<Dictionary<String, AttributeValue>>> ScanAsync(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName = _tableName;
            scanRequest.AttributesToGet = returnAttributes;
            Dictionary<String, Condition> filter = new Dictionary<String, Condition>();
            var groupedAttributes = item.GetAttributesGroupedByName();
            foreach (var attributeGroup in groupedAttributes)
            {
                Condition condition = new Condition();
                switch (comparison)
                {
                    case ComparisonOperator.Enum.Equal:
                    case ComparisonOperator.Enum.NotEqual:
                    case ComparisonOperator.Enum.LessThanOrEqual:
                    case ComparisonOperator.Enum.LessThan:
                    case ComparisonOperator.Enum.GreaterThanOrEqual:
                    case ComparisonOperator.Enum.GreaterThan:
                    case ComparisonOperator.Enum.ChecksForASubsequenceOrValueInASet:
                    case ComparisonOperator.Enum.ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet:
                    case ComparisonOperator.Enum.ChecksForAPrefix:
                    case ComparisonOperator.Enum.ChecksForExactMatches:
                    case ComparisonOperator.Enum.GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue:
                        List<AttributeValue> attributeValueList = new List<AttributeValue>();
                        foreach (var value in attributeGroup.Value)
                        {
                            attributeValueList.Add(value.ToAttributeValue());
                        }
                        condition.AttributeValueList = attributeValueList;
                        break;
                }
                condition.ComparisonOperator = ComparisonOperator.String[comparison];
                filter[attributeGroup.Key] = condition;
            }
            scanRequest.ScanFilter = filter;
            ScanResponse scanResponse = await _ddb.ScanAsync(scanRequest);
            return scanResponse.Items;
        }
        #endregion
    }
}
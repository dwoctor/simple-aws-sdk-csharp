using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBTable
    /// </summary>
    public class DynamoDBTable
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
        /// <param name="awsCredentials">AWS Credentials</param>
        public DynamoDBTable(AWSCredentials awsCredentials, String tableName)
        {
			try
			{
                _ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
				_tableName = tableName;
				_writeBatch = new DynamoDBBatchWrite(_tableName);
			}
			catch (Exception error)
			{
                throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
				if (item.HasHashKey)
				{
					_writeBatch.Put(item);
				}
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey.ToString());
				}
			}
			catch (Exception error)
			{			
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Adds an item to be deleted in the next batch write.
        /// </summary>
        /// <param name="itemPrimaryKey">The items primary key.</param>
        public void AddToBatchWriteDelete(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
	            if (itemPrimaryKey.IsHashKey)
	            {
					_writeBatch.Delete(itemPrimaryKey);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a table associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="attribute">hash key</param>
        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
        public static DynamoDBTable CreateTable(AWSCredentials awsCredentials, String tableName, DynamoDBAttribute attribute, Int64 readCapacityUnits, Int64 writeCapacityUnits)
        {
			try
			{
                if (GetDictionaryOfTables(awsCredentials).ContainsKey(tableName) == false)
				{
                    AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
					List<KeySchemaElement> keySchema = new List<KeySchemaElement>() { attribute.KeySchemaElement };
					ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput() { ReadCapacityUnits = readCapacityUnits, WriteCapacityUnits = writeCapacityUnits };
                    List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>() { attribute.AttributeDefinition };
                    CreateTableRequest createTableRequest = new CreateTableRequest() { TableName = tableName, KeySchema = keySchema, ProvisionedThroughput = provisionedThroughput, AttributeDefinitions = attributeDefinitions };
					CreateTableResponse createTableResponse = ddb.CreateTable(createTableRequest);
                    return new DynamoDBTable(awsCredentials, tableName);
				}
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.TableAlreadyExists);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="tableName">The name of the table to delete</param>
        public static void DeleteTable(AWSCredentials awsCredentials, String tableName)
        {
			try
			{
                if (GetDictionaryOfTables(awsCredentials).ContainsKey(tableName))
	            {
                    AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
					DeleteTableRequest deleteTableRequest = new DeleteTableRequest() { TableName = tableName };
					DeleteTableResponse deleteTableResponse = ddb.DeleteTable(deleteTableRequest);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.TableDoesNotExist);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets a list of tables associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static List<DynamoDBTable> GetListOfTables(AWSCredentials awsCredentials)
        {
			try
			{
	            List<DynamoDBTable> tables = new List<DynamoDBTable>();
                AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
	            ListTablesRequest listTablesRequest = new ListTablesRequest();
	            ListTablesResponse listTablesResponse = ddb.ListTables(listTablesRequest);
	            foreach (var tableName in listTablesResponse.TableNames)
	            {
                    tables.Add(new DynamoDBTable(awsCredentials, tableName));
	            }
	            return tables;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets a dictionary of tables associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        public static Dictionary<String, DynamoDBTable> GetDictionaryOfTables(AWSCredentials awsCredentials)
        {
			try
			{
	            Dictionary<String, DynamoDBTable> tables = new Dictionary<String, DynamoDBTable>();
                AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
	            ListTablesRequest listTablesRequest = new ListTablesRequest();
	            ListTablesResponse listTablesResponse = ddb.ListTables(listTablesRequest);
                foreach (var tableName in listTablesResponse.TableNames)
	            {
                    tables.Add(tableName, new DynamoDBTable(awsCredentials, tableName));
	            }
	            return tables;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        #region PutItem
        /// <summary>
        /// Puts an item in the table
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public void PutItem(DynamoDBItem item)
        {
			try
			{
	            if (item.HasHashKey)
	            {
	                PutItemRequest putItemRequest = new PutItemRequest();
	                putItemRequest.TableName = _tableName;
	                putItemRequest.Item = item.ToDictionary();
	                _ddb.PutItem(putItemRequest);
	            }
	            else
	            {
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
	            }
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region UpdateItem
        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attribute">The item's attribute to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public void UpdateItem(DynamoDBAttribute itemPrimaryKey, DynamoDBAttribute attribute, UpdateAction action)
        {
			try
			{
	            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
	            item.Add(attribute);
	            UpdateItem(item, action);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributes">The item's attributes to be updated</param>
        /// <param name="action">The action to perform when updating the item's attributes</param>
        public void UpdateItem(DynamoDBAttribute itemPrimaryKey, List<DynamoDBAttribute> attributes, UpdateAction action)
        {
			try
			{
	            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
	            item.Add(attributes);
	            UpdateItem(item, action);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="item">The item to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public void UpdateItem(DynamoDBItem item, UpdateAction action)
        {
			try
			{
	            if (item.HasHashKey)
	            {
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
					UpdateItemResponse putItemResponse = _ddb.UpdateItem(updateItemRequest);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region BatchWrite
        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        public void BatchWriteExecute()
        {
			try
			{
	            BatchWriteExecute(_writeBatch);
	            _writeBatch.Reset();
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        private void BatchWriteExecute(DynamoDBBatchWrite writeBatch)
        {
			try
			{
	            foreach (var subBatch in writeBatch.Batch)
	            {
	                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
	                batchWriteItemRequest.RequestItems = subBatch;
	                BatchWriteItemResponse batchWriteItemResponse = _ddb.BatchWriteItem(batchWriteItemRequest);
	            }
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items to be added
        /// </summary>
        /// <param name="data">List of items containing their attributes to be added</param>
        public void BatchWritePutExecute(List<DynamoDBItem> data)
        {
			try
			{
	            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
	            foreach (var item in data)
	            {
	                if (item.HasHashKey)
					{
						writeBatch.Put(item);
					}
					else
	                {
						throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
	                }
	            }
	            BatchWriteExecute(writeBatch);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items to be deleted
        /// </summary>
        /// <param name="data">List of items containing their primary key to be deleted</param>
        public void BatchWriteDeleteExecute(List<DynamoDBAttribute> data)
        {
			try
			{
	            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
	            foreach (var primaryKey in data)
	            {
	                if (primaryKey.IsHashKey)
					{
						writeBatch.Delete(primaryKey);
					}
					else
	                {
						throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
	                }
	            }
	            BatchWriteExecute(writeBatch);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region DeleteItem
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
	            if (itemPrimaryKey.IsHashKey)
	            {
	                DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
	                deleteItemRequest.TableName = _tableName;
	                deleteItemRequest.Key = itemPrimaryKey.Key;
	                DeleteItemResponse deleteItemResponse = _ddb.DeleteItem(deleteItemRequest);
	            }
	            else
	            {
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
	            }
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region GetItem
        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
	            return GetItem(itemPrimaryKey, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, String returnAttribute)
        {
			try
			{
				return GetItem(itemPrimaryKey, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, IEnumerable<String> returnAttributes)
        {
			try
			{
            	return GetItem(itemPrimaryKey, returnAttributes.ToList());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem GetItem(DynamoDBAttribute itemPrimaryKey, List<String> returnAttributes)
        {
			try
			{
	            if (itemPrimaryKey.IsHashKey)
	            {
					GetItemRequest getItemRequest = new GetItemRequest();
					getItemRequest.TableName = _tableName;
					getItemRequest.Key = itemPrimaryKey.Key;
					getItemRequest.AttributesToGet = returnAttributes;
					GetItemResponse getItemResponse = _ddb.GetItem(getItemRequest);
					return new DynamoDBItem(getItemResponse.Item);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region ScanReturnItem
        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
        {
			try
			{
	            return ScanReturnItem(new DynamoDBItem(attribute), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
	            return ScanReturnItem(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
		{
			try
			{
				return ScanReturnItem (new DynamoDBItem (attribute), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return ScanReturnItem(new DynamoDBItem(attributes), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
            	return ScanReturnItem(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
			{
            	return ScanReturnItem(new DynamoDBItem(attributes), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return ScanReturnItem(item, comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
            	return ScanReturnItem(item, comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public DynamoDBItem ScanReturnItem(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
			{
				List<Dictionary<String, AttributeValue>> scanResult = Scan(item, comparison, returnAttributes);
				return new DynamoDBItem(scanResult);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
	    }
        #endregion

        #region ScanReturnItems
        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attribute), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attribute">The name and value of the attribute of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBAttribute attribute, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attribute), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attributes), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
			{
            	return ScanReturnItems(new DynamoDBItem(attributes), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return ScanReturnItems(item, comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttribute">The attribute of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison, String returnAttribute)
        {
			try
			{
            	return ScanReturnItems(item, comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding an item in the table</param>
        /// <returns>Multiple items with their attributes</returns>
        public List<DynamoDBItem> ScanReturnItems(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
			{
	            List<Dictionary<String, AttributeValue>> scanResult = Scan(item, comparison, returnAttributes);
	            List<DynamoDBItem> returnItems = new List<DynamoDBItem>();
	            foreach (var scanResultItem in scanResult)
	            {
	                returnItems.Add(new DynamoDBItem(scanResultItem));
	            }
	            return returnItems;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>The Scan Result</returns>
        private List<Dictionary<String, AttributeValue>> Scan(DynamoDBItem item, ComparisonOperator.Enum comparison, List<String> returnAttributes)
        {
			try
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
	            ScanResponse scanResponse = _ddb.Scan(scanRequest);
	            return scanResponse.Items;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
#endif
        #endregion

        #region Async
#if NET45
        /// <summary>
        /// Creates a table associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="attribute">hash key</param>
        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
        public static async Task<DynamoDBTable> CreateTableAsync(AWSCredentials awsCredentials, String tableName, DynamoDBAttribute attribute, Int64 readCapacityUnits, Int64 writeCapacityUnits)
        {
			try
			{
                Dictionary<String, DynamoDBTable> tables = await GetDictionaryOfTablesAsync(awsCredentials);
	            if (tables.ContainsKey(tableName) == false)
	            {
                    AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
                    List<KeySchemaElement> keySchema = new List<KeySchemaElement>() { attribute.KeySchemaElement };
                    ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput() { ReadCapacityUnits = readCapacityUnits, WriteCapacityUnits = writeCapacityUnits };
                    List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>() { attribute.AttributeDefinition };
                    CreateTableRequest createTableRequest = new CreateTableRequest() { TableName = tableName, KeySchema = keySchema, ProvisionedThroughput = provisionedThroughput, AttributeDefinitions = attributeDefinitions };
                    CreateTableResponse createTableResponse = await ddb.CreateTableAsync(createTableRequest);
                    return new DynamoDBTable(awsCredentials, tableName);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.TableAlreadyExists);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="tableName">The name of the table to delete</param>
        public static async Task DeleteTableAsync(AWSCredentials awsCredentials, String tableName)
        {
			try
			{
	            Dictionary<String, DynamoDBTable> tables = await GetDictionaryOfTablesAsync(awsCredentials);
				if (tables.ContainsKey(tableName) == true)
				{
                    AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
					DeleteTableRequest deleteTableRequest = new DeleteTableRequest() { TableName = tableName };
					DeleteTableResponse deleteTableResponse = await ddb.DeleteTableAsync(deleteTableRequest);
				}
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.TableDoesNotExist);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets a list of tables associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        public static async Task<List<DynamoDBTable>> GetListOfTablesAsync(AWSCredentials awsCredentials)
		{
			try
			{
				List<DynamoDBTable> tables = new List<DynamoDBTable> ();
                AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
				ListTablesRequest listTablesRequest = new ListTablesRequest ();
				ListTablesResponse listTablesResponse = await ddb.ListTablesAsync (listTablesRequest);
				foreach (var item in listTablesResponse.TableNames)
                {
                    tables.Add(new DynamoDBTable(awsCredentials, item));
				}
				return tables;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets a dictionary of tables associated with an AWS account
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static async Task<Dictionary<String, DynamoDBTable>> GetDictionaryOfTablesAsync(AWSCredentials awsCredentials)
        {
			try
			{
	            Dictionary<String, DynamoDBTable> tables = new Dictionary<String, DynamoDBTable>();
                AmazonDynamoDBClient ddb = new AmazonDynamoDBClient(awsCredentials.AccessKey, awsCredentials.SecretAccessKey, awsCredentials.Region);
	            ListTablesRequest listTablesRequest = new ListTablesRequest();
	            ListTablesResponse listTablesResponse = await ddb.ListTablesAsync(listTablesRequest);
	            foreach (var item in listTablesResponse.TableNames)
	            {
                    tables.Add(item, new DynamoDBTable(awsCredentials, item));
	            }
	            return tables;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        #region PutItemAsync
        /// <summary>
        /// Puts an item in the table
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public async Task PutItemAsync(DynamoDBItem item)
        {
			try
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
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
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
        public async Task UpdateItemAsync(DynamoDBAttribute itemPrimaryKey, DynamoDBAttribute attribute, UpdateAction action)
        {
			try
			{
	            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
	            item.Add(attribute);
	            await UpdateItemAsync(item, action);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="attributes">The item's attributes to be updated</param>
        /// <param name="action">The action to perform when updating the item's attributes</param>
        public async Task UpdateItemAsync(DynamoDBAttribute itemPrimaryKey, List<DynamoDBAttribute> attributes, UpdateAction action)
        {
			try
			{
	            DynamoDBItem item = new DynamoDBItem(itemPrimaryKey);
	            item.Add(attributes);
	            await UpdateItemAsync(item, action);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Updates an item in the table
        /// </summary>
        /// <param name="item">The item to be updated</param>
        /// <param name="action">The action to perform when updating the item's attribute</param>
        public async Task UpdateItemAsync(DynamoDBItem item, UpdateAction action)
        {
			try
			{
	            if (item.HasHashKey)
	            {
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
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region BatchWriteAsync
        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        public async Task BatchWriteExecuteAsync()
        {
			try
			{
	            await BatchWriteExecuteAsync(_writeBatch);
	            _writeBatch.Reset();
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items already added to the batch
        /// </summary>
        private async Task BatchWriteExecuteAsync(DynamoDBBatchWrite writeBatch)
        {
			try
			{
	            foreach (var subBatch in writeBatch.Batch)
	            {
	                BatchWriteItemRequest batchWriteItemRequest = new BatchWriteItemRequest();
	                batchWriteItemRequest.RequestItems = subBatch;
	                BatchWriteItemResponse batchWriteItemResponse = await _ddb.BatchWriteItemAsync(batchWriteItemRequest);
	            }
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items to be added
        /// </summary>
        /// <param name="data">List of items containing their attributes to be added</param>
        public async Task BatchWritePutExecuteAsync(List<DynamoDBItem> data)
        {
			try
			{
	            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
	            foreach (var item in data)
	            {
	                if (item.HasHashKey)
	                {
						writeBatch.Put(item);
	                }
	                else
					{
						throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBItemHasNoHashKey);
					}
	            }
	            await BatchWriteExecuteAsync(writeBatch);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Performs a batch write on items to be deleted
        /// </summary>
        /// <param name="data">List of items containing their primary key to be deleted</param>
        public async Task BatchWriteDeleteExecuteAsync(List<DynamoDBAttribute> data)
        {
			try
			{
	            DynamoDBBatchWrite writeBatch = new DynamoDBBatchWrite(_tableName);
	            foreach (var primaryKey in data)
	            {
	                if (primaryKey.IsHashKey)
	                {
						writeBatch.Delete(primaryKey);	                }
					else
					{
						throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
					}
	            }
	            await BatchWriteExecuteAsync(writeBatch);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
        #endregion

        #region DeleteItemAsync
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public async Task DeleteItemAsync(DynamoDBAttribute itemPrimaryKey)
        {
			try
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
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
	            }
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
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
			try
			{
            	return await GetItemAsync(itemPrimaryKey, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, String returnAttribute)
        {
			try
			{
            	return await GetItemAsync(itemPrimaryKey, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, IEnumerable<String> returnAttributes)
        {
			try
			{
            	return await GetItemAsync(itemPrimaryKey, returnAttributes.ToList());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        /// <param name="returnAttributes">The attributes of the item to to be returned upon finding the item in the table</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBAttribute itemPrimaryKey, List<String> returnAttributes)
        {
			try
			{
	            if (itemPrimaryKey.IsHashKey)
	            {
					GetItemRequest getItemRequest = new GetItemRequest();
					getItemRequest.TableName = _tableName;
					getItemRequest.Key = itemPrimaryKey.Key;
					if (returnAttributes.Count > 0)
					{
						getItemRequest.AttributesToGet = returnAttributes;
					}
					GetItemResponse getItemResponse = await _ddb.GetItemAsync(getItemRequest);
					return new DynamoDBItem(getItemResponse.Item);
	            }
				else
				{
					throw DynamoDBTableException.Generator(DynamoDBTableException.Phases.DynamoDBAttributeIsNotAHashKey);
				}
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
	            return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemAsync(new DynamoDBItem(attribute), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemAsync(new DynamoDBItem(attributes), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an item in the table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>An item's attributes</returns>
        public async Task<DynamoDBItem> ScanReturnItemAsync(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return await ScanReturnItemAsync(item, comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemAsync(item, comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	List<Dictionary<String, AttributeValue>> scanResult = await ScanAsync(item, comparison, returnAttributes);
            	return new DynamoDBItem(scanResult);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attribute), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="attributes">The attributes of the item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(List<DynamoDBAttribute> attributes, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(new DynamoDBItem(attributes), comparison, returnAttributes);
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }

        /// <summary>
        /// Scans for an items in a table
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <param name="comparison">The type of comparison to perform when comparing the value of the attribute</param>
        /// <returns>Multiple items with their attributes</returns>
        public async Task<List<DynamoDBItem>> ScanReturnItemsAsync(DynamoDBItem item, ComparisonOperator.Enum comparison)
        {
			try
			{
            	return await ScanReturnItemsAsync(item, comparison, new List<String>());
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
            	return await ScanReturnItemsAsync(item, comparison, new List<String>() { returnAttribute });
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
			{
	            List<Dictionary<String, AttributeValue>> scanResult = await ScanAsync(item, comparison, returnAttributes);
	            List<DynamoDBItem> returnItems = new List<DynamoDBItem>();
	            foreach (var scanResultItem in scanResult)
	            {
	                returnItems.Add(new DynamoDBItem(scanResultItem));
	            }
	            return returnItems;
			}
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
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
			try
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
			catch (Exception error)
			{
				    throw DynamoDBTableException.Generator(error);
			}
        }
#endif
        #endregion

        #endregion
    }
}
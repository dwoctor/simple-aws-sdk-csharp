using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AWS.Tests.Library
{
    /// <summary>
	/// DynamoDBTable Tests
    /// </summary>
    public class DynamoDBTableTests : DynamoDBTests
    {
		/// <summary>
		/// Tests GetListOfTables
		/// </summary>y>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
        public Boolean GetListOfTables()
        {
            try
            {
                DynamoDBTable.GetListOfTables(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
            }
            catch
            {
				return false;
            }
			return true;
        }

        /// <summary>
        /// Tests GetDictionaryOfTables
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean GetDictionaryOfTables()
        {
            try
            {
                DynamoDBTable.GetDictionaryOfTables(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
            }
            catch
            {
				return false;
            }
			return true;
        }

        /// <summary>
        /// Tests CreateTable
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean CreateTable()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            try
            {
                DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
			}
			catch (DynamoDBTableException){ }
            catch
            {
				return false;
            }
			return true;
        }

        /// <summary>
        /// Tests DeleteTable
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
        public Boolean DeleteTable()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            try
            {
                try
                {
                    DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBTableException) { }
                try
                {
                    DynamoDBTable.DeleteTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBTableException) { }
            }
            catch
            {
				return false;
			}
			return true;
        }

        /// <summary>
        /// Tests Constructor
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
        public Boolean Constructor()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            try
            {
                DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
            }
            catch
            {
				return false;
            }
            try
            {
                DynamoDBTable.DeleteTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
			return true;
        }

        /// <summary>
        /// Tests PutItem
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
        public Boolean PutItem()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                ddbt.PutItem(item);
                Thread.Sleep(60000);
            }
            catch
            {
				return false;
			}
            try
            {
                DynamoDBTable.DeleteTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
			return true;
        }

        /// <summary>
        /// Tests GetItem
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean GetItem()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            Int32 hashKeyValue = _hashKeyValue;
            String stringAttributeKey = _stringAttributeKey;
            String numberAttributeKey = _numberAttributeKey;
            String binaryAttributeKey = _binaryAttributeKey;
            String stringAttributeValue = _stringAttributeValue;
            Int32 numberAttributeValue = _numberAttributeValue;
            Byte[] binaryAttributeValue = _binaryAttributeValue;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                ddbt.PutItem(item);
                DynamoDBItem gotItem1 = ddbt.GetItem(item[hashKeyName]); // Returns all attributes
                DynamoDBItem gotItem2 = ddbt.GetItem(item[hashKeyName], item[hashKeyName].Name); // Returns hashKeyName attribute
                DynamoDBItem gotItem3 = ddbt.GetItem(item[hashKeyName], item.AttributeNames.Where(x => x != hashKeyName)); // Returns all attributes except hashKeyName
                if (gotItem1.ContainsAttribute(hashKeyName) == false && Convert.ToInt32(gotItem1[hashKeyName]) != hashKeyValue && gotItem1.ContainsAttribute(stringAttributeKey) == false && gotItem1[stringAttributeKey].Value as String != stringAttributeValue && gotItem1.ContainsAttribute(numberAttributeKey) == false && Convert.ToInt32(gotItem1[numberAttributeKey].Value) != numberAttributeValue && gotItem1[binaryAttributeKey].Value as Byte[] != binaryAttributeValue && gotItem1.Count != 4 &&
                    gotItem2.ContainsAttribute(stringAttributeKey) == false && gotItem2[stringAttributeKey].Value as String != stringAttributeValue && gotItem2.Count != 1 &&
                    gotItem3.ContainsAttribute(stringAttributeKey) == false && gotItem3[stringAttributeKey].Value as String != stringAttributeValue && gotItem3.ContainsAttribute(numberAttributeKey) == false && Convert.ToInt32(gotItem3[numberAttributeKey].Value) != numberAttributeValue && gotItem3[binaryAttributeKey].Value as Byte[] != binaryAttributeValue && gotItem3.Count != 3 && gotItem3.Count != 3)
                {
					return false;
				}
                Thread.Sleep(60000);
            }
            catch
            {
				return false;
            }
            try
            {
                DynamoDBTable.DeleteTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
			return true;
        }

        /// <summary>
        /// Tests UpdateItem
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean UpdateItem()
        {
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            Int32 hashKeyValue = _hashKeyValue;
            DynamoDBAttribute hashKeyAttribute = _hashKeyAttribute;
            DynamoDBAttribute stringAttribute = _stringAttribute;
            DynamoDBAttribute numberAttribute = _numberAttribute;
            DynamoDBAttribute binaryAttribute = _binaryAttribute;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                ddbt.PutItem(item);
                ddbt.UpdateItem(hashKeyAttribute, new DynamoDBAttribute(numberAttribute.Name, 1), UpdateAction.ADD);
                ddbt.UpdateItem(hashKeyAttribute, new DynamoDBAttribute(numberAttribute.Name, 3), UpdateAction.PUT);
                ddbt.UpdateItem(hashKeyAttribute, new DynamoDBAttribute(numberAttribute.Name, 1), UpdateAction.DELETE);
                Thread.Sleep(60000);
            }
            catch
            {
				return false;
            }
            try
            {
                DynamoDBTable.DeleteTable(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
			return true;
        }
    }
}
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
                DynamoDBTable.GetListOfTables(TestCredentials.Credentials);
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
                DynamoDBTable.GetDictionaryOfTables(TestCredentials.Credentials);
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
            DynamoDBAttribute hashKey = _hashKeyAttributeNoValue;
            try
            {
                DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
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
            DynamoDBAttribute hashKey = _hashKeyAttributeNoValue;
            try
            {
                try
                {
                    DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBTableException) { }
                try
                {
                    DynamoDBTable.DeleteTable(TestCredentials.Credentials, tableName);
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
            DynamoDBAttribute hashKey = _hashKeyAttributeNoValue;
            try
            {
                DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(TestCredentials.Credentials, tableName);
            }
            catch
            {
				return false;
            }
            try
            {
                DynamoDBTable.DeleteTable(TestCredentials.Credentials, tableName);
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
            DynamoDBAttribute hashKey = _hashKeyAttributeNoValue;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(TestCredentials.Credentials, tableName);
                ddbt.PutItem(item);
                Thread.Sleep(60000);
            }
            catch
            {
				return false;
			}
            try
            {
                DynamoDBTable.DeleteTable(TestCredentials.Credentials, tableName);
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
            DynamoDBAttribute hashKey = _hashKeyAttribute;
            String stringAttributeKey = _stringAttributeKey;
            String numberAttributeKey = _numberAttributeKey;
            String binaryAttributeKey = _binaryAttributeKey;
            String stringAttributeValue = _stringAttributeValue;
            Int32 numberAttributeValue = _numberAttributeValue;
            Byte[] binaryAttributeValue = _binaryAttributeValue;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(TestCredentials.Credentials, tableName);
                ddbt.PutItem(item);
                DynamoDBItem gotItem1 = ddbt.GetItem(item[hashKey.Name]); // Returns all attributes
                DynamoDBItem gotItem2 = ddbt.GetItem(item[hashKey.Name], item[hashKey.Name].Name); // Returns hashKeyName attribute
                DynamoDBItem gotItem3 = ddbt.GetItem(item[hashKey.Name], item.AttributeNames.Where(x => x != hashKey.Name)); // Returns all attributes except hashKeyName
                if (gotItem1.ContainsAttribute(hashKey.Name) == false && Convert.ToInt32(gotItem1[hashKey.Name]) != Convert.ToInt32(hashKey.Value) && gotItem1.ContainsAttribute(stringAttributeKey) == false && gotItem1[stringAttributeKey].Value as String != stringAttributeValue && gotItem1.ContainsAttribute(numberAttributeKey) == false && Convert.ToInt32(gotItem1[numberAttributeKey].Value) != numberAttributeValue && gotItem1[binaryAttributeKey].Value as Byte[] != binaryAttributeValue && gotItem1.Count != 4 &&
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
                DynamoDBTable.DeleteTable(TestCredentials.Credentials, tableName);
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
            DynamoDBAttribute hashKey = _hashKeyAttribute;
            DynamoDBAttribute numberAttribute = _numberAttribute;
            DynamoDBItem item = _item;
            try
            {
                DynamoDBTable.CreateTable(TestCredentials.Credentials, tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
            try
            {
                DynamoDBTable ddbt = new DynamoDBTable(TestCredentials.Credentials, tableName);
                ddbt.PutItem(item);
                ddbt.UpdateItem(hashKey, new DynamoDBAttribute(numberAttribute.Name, 1), UpdateAction.ADD);
                ddbt.UpdateItem(hashKey, new DynamoDBAttribute(numberAttribute.Name, 3), UpdateAction.PUT);
                ddbt.UpdateItem(hashKey, new DynamoDBAttribute(numberAttribute.Name, 1), UpdateAction.DELETE);
                Thread.Sleep(60000);
            }
            catch
            {
				return false;
            }
            try
            {
                DynamoDBTable.DeleteTable(TestCredentials.Credentials, tableName);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
			return true;
        }
    }
}
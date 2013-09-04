using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AWS.Tests.Library
{
	/// <summary>
	/// DynamoDBTableCache Tests
	/// </summary>
	public class DynamoDBTableCacheTests : DynamoDBTests
	{
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
				DynamoDBTableCache ddbt = new DynamoDBTableCache(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
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
				DynamoDBTableCache ddbtc = new DynamoDBTableCache(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
				ddbtc.PutItem(item);
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
				DynamoDBTableCache ddbtc = new DynamoDBTableCache(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey, tableName);
				ddbtc.PutItem(item);
				DynamoDBItem gotItem1 = ddbtc.GetItem(item); // Returns all attributes
				if (gotItem1.ContainsAttribute(hashKeyName) == false && Convert.ToInt32(gotItem1[hashKeyName].Value) != hashKeyValue && gotItem1.ContainsAttribute(stringAttributeKey) == false && gotItem1[stringAttributeKey].Value as String != stringAttributeValue && gotItem1.ContainsAttribute(numberAttributeKey) == false && Convert.ToInt32(gotItem1[numberAttributeKey].Value) != numberAttributeValue && gotItem1[binaryAttributeKey].Value as Byte[] != binaryAttributeValue && gotItem1.Count != 4)
				{
					return false;
				}
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
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AWS.Tests.Library
{
    /// <summary>
    /// DynamoDBClient Tests
    /// </summary>
    public class DynamoDBClientTests : DynamoDBTests
    {
        /// <summary>
        /// Tests the constructor
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean Constructor()
        {
            try
            {
                AWS.DynamoDB.DynamoDBClient ddb = new AWS.DynamoDB.DynamoDBClient(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
            }
            catch
            {
				return false;
            }
			return true;
        }

        /// <summary>
        /// Tests the index
        /// </summary>
		/// <returns><c>true</c>, if passes, <c>false</c> if fails.</returns>
		public Boolean Index()
        {
            try
            {
                AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
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
                catch
                {
					return false;
                }
                DynamoDBTable table = ddb[tableName];
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
                AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
                ddb.CreateTable(tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                Thread.Sleep(60000);
            }
            catch (DynamoDBTableException) { }
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
            AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(AWSCredentials.AccessKey, AWSCredentials.SecretAccessKey);
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            String hashKeyName = _hashKeyName;
            try
            {
                try
                {
                    ddb.CreateTable(tableName, hashKeyName, Types.Enum.Number, readCapacityUnits, writeCapacityUnits);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBTableException) { }
                try
                {
                    ddb.DeleteTable(tableName);
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
    }
}
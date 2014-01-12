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
                AWS.DynamoDB.DynamoDBClient ddb = new AWS.DynamoDB.DynamoDBClient(TestCredentials.Credentials);
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
                AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(TestCredentials.Credentials);
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
                catch
                {
					return false;
                }
                DynamoDBTable table = ddb[tableName];
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
                AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(TestCredentials.Credentials);
                ddb.CreateTable(tableName, hashKey, readCapacityUnits, writeCapacityUnits);
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
            AWS.DynamoDB.DynamoDBClient ddb = new DynamoDB.DynamoDBClient(TestCredentials.Credentials);
            String tableName = _tableName;
            Int64 readCapacityUnits = _readCapacityUnits;
            Int64 writeCapacityUnits = _writeCapacityUnits;
            DynamoDBAttribute hashKey = _hashKeyAttributeNoValue;

            try
            {
                try
                {
                    ddb.CreateTable(tableName, hashKey, readCapacityUnits, writeCapacityUnits);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBClientException) { }
                try
                {
                    ddb.DeleteTable(tableName);
                    Thread.Sleep(60000);
                }
                catch (DynamoDBClientException) { }
            }
            catch
            {
				return false;
            }
			return true;
        }
    }
}
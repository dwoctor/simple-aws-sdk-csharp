using AWS.DynamoDB;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.DynamoDB
{
    /// <summary>
    /// DynamoDBClient Tests
    /// </summary>
    [TestClass]
    public class DynamoDBClientTests : DynamoDBTests
    {
        /// <summary>
        /// Tests the constructor
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            DynamoDBClient ddbClient = new DynamoDBClient(this.TestData.Credentials);
        }

        /// <summary>
        /// Tests the index
        /// </summary>
        [TestMethod]
        public void Index()
        {
            this.TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBClient ddbClient = this.TestData.DDBClient;
            DynamoDBTable table = ddbClient[this.TestData.TableName];
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests CreateTable and DeleteTable
        /// </summary>
        [TestMethod]
        public void CreateAndDeleteTable()
        {
            this.TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests CreateTableAsync and DeleteTableAsync
        /// </summary>
        [TestMethod]
        public async Task CreateAndDeleteTableAsync()
        {
            this.TestData.GenerateNewTableName();
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests DeleteTable
        /// </summary>
        [TestMethod]
        public void DeleteNonExistentTable()
        {
            try
            {
                this.TestData.GenerateNewTableName();
                DynamoDBClient ddbClient = this.TestData.DDBClient;
                ddbClient.DeleteTable(this.TestData.TableName);
                Assert.Fail();
            }
            catch { }
        }

        /// <summary>
        /// Tests GetTables
        /// </summary>
        [TestMethod]
        public void GetTables()
        {
            DynamoDBClient ddbClient = this.TestData.DDBClient;
            ddbClient.GetTables();
        }

        /// <summary>
        /// Tests GetTablesAsync
        /// </summary>
        [TestMethod]
        public async Task GetTablesAsync()
        {
            DynamoDBClient ddbClient = this.TestData.DDBClient;
            await ddbClient.GetTablesAsync();
        }
    }
}
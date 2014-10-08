using AWS.DynamoDB;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.DynamoDB
{
    [TestClass]
    public class DynamoDBTableCacheTests : DynamoDBTests
    {
        /// <summary>
        /// Tests Constructor
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            DynamoDBTableCache ddbtcClient = new DynamoDBTableCache(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests PutItem
        /// </summary>
        [TestMethod]
        public void PutItem()
        {
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTableCache ddbtcClient = this.TestData.DDBTableCache;
            ddbtcClient.PutItem(this.TestData.DDBItem);
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests PutItemAsync
        /// </summary>
        [TestMethod]
        public async Task PutItemAsync()
        {
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTableCache ddbtcClient = this.TestData.DDBTableCache;
            await ddbtcClient.PutItemAsync(this.TestData.DDBItem);
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests GetItem
        /// </summary>
        [TestMethod]
        public void GetItem()
        {
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTableCache ddbtcClient = this.TestData.DDBTableCache;
            ddbtcClient.PutItem(this.TestData.DDBItem);
            DynamoDBItem gotItem1 = ddbtcClient.GetItem(this.TestData.DDBItem); // Returns all attributes
            if (gotItem1.ContainsAttribute(this.TestData.HashKeyAttribute.Name) == false &&
                Convert.ToInt32(gotItem1[this.TestData.HashKeyAttribute.Name].Value) != Convert.ToInt32(this.TestData.HashKeyAttribute.Value) &&
                gotItem1.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem1[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem1.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem1[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem1[this.TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem1.Count != 4)
            {
                Assert.Fail();
            }
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests GetItemAsync
        /// </summary>
        [TestMethod]
        public async Task GetItemAsync()
        {
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTableCache ddbtcClient = this.TestData.DDBTableCache;
            await ddbtcClient.PutItemAsync(this.TestData.DDBItem);
            DynamoDBItem gotItem1 = await ddbtcClient.GetItemAsync(this.TestData.DDBItem); // Returns all attributes
            if (gotItem1.ContainsAttribute(this.TestData.HashKeyAttribute.Name) == false &&
                Convert.ToInt32(gotItem1[this.TestData.HashKeyAttribute.Name].Value) != Convert.ToInt32(this.TestData.HashKeyAttribute.Value) &&
                gotItem1.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem1[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem1.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem1[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem1[this.TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem1.Count != 4)
            {
                Assert.Fail();
            }
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }
    }
}
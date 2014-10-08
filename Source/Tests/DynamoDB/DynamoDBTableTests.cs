using AWS.DynamoDB;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.DynamoDB
{
    [TestClass]
    public class DynamoDBTableTests : DynamoDBTests
    {
        /// <summary>
        /// Tests GetListOfTables
        /// </summary>
        [TestMethod]
        public void GetListOfTables()
        {
            DynamoDBTable.GetListOfTables(this.TestData.Credentials);
        }

        /// <summary>
        /// Tests GetListOfTablesAsync
        /// </summary>
        [TestMethod]
        public async Task GetListOfTablesAsync()
        {
            await DynamoDBTable.GetListOfTablesAsync(this.TestData.Credentials);
        }

        /// <summary>
        /// Tests GetDictionaryOfTables
        /// </summary>
        [TestMethod]
        public void GetDictionaryOfTables()
        {
            DynamoDBTable.GetDictionaryOfTables(this.TestData.Credentials);
        }

        /// <summary>
        /// Tests GetDictionaryOfTablesAsync
        /// </summary>
        [TestMethod]
        public async Task GetDictionaryOfTablesAsync()
        {
            await DynamoDBTable.GetDictionaryOfTablesAsync(this.TestData.Credentials);
        }

        /// <summary>
        /// Tests CreateTable and DeleteTable
        /// </summary>
        [TestMethod]
        public void CreateAndDeleteTable()
        {
            this.TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(TestData.WaitTime);
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
            Thread.Sleep(TestData.WaitTime);
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests Constructor
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            DynamoDBTable ddbt = new DynamoDBTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests PutItem
        /// </summary>
        [TestMethod]
        public void PutItem()
        {
            this.TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            ddbt.PutItem(this.TestData.DDBItem);
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests PutItemAsync
        /// </summary>
        [TestMethod]
        public async Task PutItemAsync()
        {
            this.TestData.GenerateNewTableName();
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            await ddbt.PutItemAsync(this.TestData.DDBItem);
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests GetItem
        /// </summary>
        [TestMethod]
        public void GetItem()
        {
            TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            ddbt.PutItem(this.TestData.DDBItem);
            DynamoDBItem gotItem1 = ddbt.GetItem(this.TestData.HashKeyAttribute); // Returns all attributes
            DynamoDBItem gotItem2 = ddbt.GetItem(this.TestData.HashKeyAttribute, this.TestData.HashKeyAttribute.Name); // Returns hashKeyName attribute
            DynamoDBItem gotItem3 = ddbt.GetItem(this.TestData.HashKeyAttribute, this.TestData.DDBItem.AttributeNames.Where(x => x != this.TestData.HashKeyAttribute.Name)); // Returns all attributes except hashKeyName
            if (// gotItem1
                gotItem1.ContainsAttribute(this.TestData.HashKeyAttribute.Name) == false &&
                Convert.ToInt32(gotItem1[this.TestData.HashKeyAttribute.Name]) != Convert.ToInt32(this.TestData.HashKeyAttribute.Value) &&
                gotItem1.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem1[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem1.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem1[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem1[this.TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem1.Count != 4 &&
                // gotItem2
                gotItem2.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem2[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem2.Count != 1 &&
                // gotItem3
                gotItem3.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem3[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem3.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem3[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem3[TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem3.Count != 3 &&
                gotItem3.Count != 3)
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
            TestData.GenerateNewTableName();
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            await ddbt.PutItemAsync(this.TestData.DDBItem);
            DynamoDBItem gotItem1 = await ddbt.GetItemAsync(this.TestData.HashKeyAttribute); // Returns all attributes
            DynamoDBItem gotItem2 = await ddbt.GetItemAsync(this.TestData.HashKeyAttribute, this.TestData.HashKeyAttribute.Name); // Returns hashKeyName attribute
            DynamoDBItem gotItem3 = await ddbt.GetItemAsync(this.TestData.HashKeyAttribute, this.TestData.DDBItem.AttributeNames.Where(x => x != this.TestData.HashKeyAttribute.Name)); // Returns all attributes except hashKeyName
            if (// gotItem1
                gotItem1.ContainsAttribute(this.TestData.HashKeyAttribute.Name) == false &&
                Convert.ToInt32(gotItem1[this.TestData.HashKeyAttribute.Name]) != Convert.ToInt32(this.TestData.HashKeyAttribute.Value) &&
                gotItem1.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem1[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem1.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem1[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem1[this.TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem1.Count != 4 &&
                // gotItem2
                gotItem2.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem2[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem2.Count != 1 &&
                // gotItem3
                gotItem3.ContainsAttribute(this.TestData.StringAttributeKey) == false &&
                gotItem3[this.TestData.StringAttributeKey].Value as String != this.TestData.StringAttributeValue &&
                gotItem3.ContainsAttribute(this.TestData.NumberAttributeKey) == false &&
                Convert.ToInt32(gotItem3[this.TestData.NumberAttributeKey].Value) != this.TestData.NumberAttributeValue &&
                gotItem3[TestData.BinaryAttributeKey].Value as Byte[] != this.TestData.BinaryAttributeValue &&
                gotItem3.Count != 3 &&
                gotItem3.Count != 3)
            {
                Assert.Fail();
            }
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests UpdateItem
        /// </summary>
        [TestMethod]
        public void UpdateItem()
        {
            TestData.GenerateNewTableName();
            DynamoDBTable.CreateTable(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            ddbt.PutItem(this.TestData.DDBItem);
            ddbt.UpdateItem(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 1), UpdateAction.ADD);
            ddbt.UpdateItem(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 3), UpdateAction.PUT);
            ddbt.UpdateItem(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 1), UpdateAction.DELETE);
            DynamoDBTable.DeleteTable(this.TestData.Credentials, this.TestData.TableName);
        }

        /// <summary>
        /// Tests UpdateItemAsync
        /// </summary>
        [TestMethod]
        public async Task UpdateItemAsync()
        {
            TestData.GenerateNewTableName();
            await DynamoDBTable.CreateTableAsync(this.TestData.Credentials, this.TestData.TableName, this.TestData.HashKeyAttributeNoValue, this.TestData.ReadCapacityUnits, this.TestData.WriteCapacityUnits);
            Thread.Sleep(this.TestData.WaitTime);
            DynamoDBTable ddbt = this.TestData.DDBTable;
            await ddbt.PutItemAsync(this.TestData.DDBItem);
            await ddbt.UpdateItemAsync(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 1), UpdateAction.ADD);
            await ddbt.UpdateItemAsync(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 3), UpdateAction.PUT);
            await ddbt.UpdateItemAsync(this.TestData.HashKeyAttribute, new DynamoDBAttribute(this.TestData.NumberAttribute.Name, 1), UpdateAction.DELETE);
            await DynamoDBTable.DeleteTableAsync(this.TestData.Credentials, this.TestData.TableName);
        }
    }
}

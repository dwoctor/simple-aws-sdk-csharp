using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.Unit
{
    /// <summary>
	/// DynamoDBTable Tests
    /// </summary>
    [TestClass]
    public class DynamoDBTableTests
    {
        AWS.Tests.Library.DynamoDBTableTests dynamoDBTableTests = new AWS.Tests.Library.DynamoDBTableTests();

		/// <summary>
		/// Tests GetListOfTables
		/// </summary>
        [TestMethod]
        public void GetListOfTables()
        {
            Assert.IsTrue(dynamoDBTableTests.GetListOfTables());
        }

        /// <summary>
        /// Tests GetDictionaryOfTables
        /// </summary>
        [TestMethod]
		public void GetDictionaryOfTables()
        {
            Assert.IsTrue(dynamoDBTableTests.GetDictionaryOfTables());
        }

        /// <summary>
        /// Tests CreateTable
        /// </summary>
        [TestMethod]
		public void CreateTable()
        {
            Assert.IsTrue(dynamoDBTableTests.CreateTable());
        }

        /// <summary>
        /// Tests DeleteTable
        /// </summary>
        [TestMethod]
        public void DeleteTable()
        {
            Assert.IsTrue(dynamoDBTableTests.DeleteTable());
        }

        /// <summary>
        /// Tests Constructor
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            Assert.IsTrue(dynamoDBTableTests.Constructor());
        }

        /// <summary>
        /// Tests PutItem
        /// </summary>
        [TestMethod]
        public void PutItem()
        {
            Assert.IsTrue(dynamoDBTableTests.PutItem());
        }

        /// <summary>
        /// Tests GetItem
        /// </summary>
        [TestMethod]
        public void GetItem()
        {
            Assert.IsTrue(dynamoDBTableTests.GetItem());
        }

        /// <summary>
        /// Tests UpdateItem
        /// </summary>
        [TestMethod]
        public void UpdateItem()
        {
            Assert.IsTrue(dynamoDBTableTests.UpdateItem());
        }
    }
}
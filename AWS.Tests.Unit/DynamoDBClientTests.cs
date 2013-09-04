using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.Unit
{
    /// <summary>
    /// DynamoDBClient Tests
    /// </summary>
    [TestClass]
    public class DynamoDBClientTests
    {
        AWS.Tests.Library.DynamoDBClientTests dynamoDBClientTests = new AWS.Tests.Library.DynamoDBClientTests();

        /// <summary>
        /// Tests the constructor
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            Assert.IsTrue(dynamoDBClientTests.Constructor());
        }

        /// <summary>
        /// Tests the index
        /// </summary>
        [TestMethod]
        public void Index()
        {
            Assert.IsTrue(dynamoDBClientTests.Index());
        }

        /// <summary>
        /// Tests CreateTable
        /// </summary>
        [TestMethod]
        public void CreateTable()
        {
            Assert.IsTrue(dynamoDBClientTests.CreateTable());
        }

        /// <summary>
        /// Tests DeleteTable
        /// </summary>
        [TestMethod]
        public void DeleteTable()
        {
            Assert.IsTrue(dynamoDBClientTests.DeleteTable());
        }
    }
}
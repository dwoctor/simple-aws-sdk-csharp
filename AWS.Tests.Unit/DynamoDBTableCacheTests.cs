using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AWS.Tests.Unit
{
	/// <summary>
	/// DynamoDBTableCache Tests
	/// </summary>
    [TestClass]
    public class DynamoDBTableCacheTests
	{
        AWS.Tests.Library.DynamoDBTableCacheTests dynamoDBTableCacheTests = new AWS.Tests.Library.DynamoDBTableCacheTests();

		/// <summary>
		/// Tests Constructor
		/// </summary>
        [TestMethod]
        public void Constructor()
		{
            Assert.IsTrue(dynamoDBTableCacheTests.Constructor());
		}

		/// <summary>
		/// Tests PutItem
		/// </summary>
        [TestMethod]
        public void PutItem()
		{
            Assert.IsTrue(dynamoDBTableCacheTests.PutItem());
		}

		/// <summary>
		/// Tests GetItem
		/// </summary>
        [TestMethod]
        public void GetItem()
		{
            Assert.IsTrue(dynamoDBTableCacheTests.GetItem());
		}
	}
}
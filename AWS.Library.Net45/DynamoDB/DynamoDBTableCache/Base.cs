using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.DynamoDB
{
	/// <summary>
	/// A DynamoDBTableCache
	/// </summary>
	public partial class DynamoDBTableCache
    {
        #region Fields
        DynamoDBTable _table;
		List<DynamoDBItem> _cachedItems = new List<DynamoDBItem>();
        #endregion

        #region Constructor
        /// <summary>
		/// Constructs a DynamoDBTableCache
		/// </summary>
		/// <param name="cacheSize">The size of the Cache in bytes (default value is 104857600 bytes or 100 mb)</param>
		public DynamoDBTableCache(String accessKey, String secretAccessKey, String tableName, UInt64 cacheSize = 104857600)
		{
			_table = new DynamoDBTable(accessKey, secretAccessKey, tableName);
		}
        #endregion
    }
}
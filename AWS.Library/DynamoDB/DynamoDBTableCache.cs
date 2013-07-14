using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;

namespace AWS.DynamoDB
{
	/// <summary>
	/// A DynamoDBTableCache
	/// </summary>
	public class DynamoDBTableCache
	{
		DynamoDBTable _table;

		List<DynamoDBItem> _cachedItems = new List<DynamoDBItem>();

		/// <summary>
		/// Constructs a DynamoDBTableCache
		/// </summary>
		/// <param name="cacheSize">The size of the Cache in bytes (default value is 104857600 bytes or 100 mb)</param>
		public DynamoDBTableCache (String accessKey, String secretAccessKey, String tableName, UInt64 cacheSize = 104857600)
		{
			_table = new DynamoDBTable(accessKey, secretAccessKey, tableName);
		}

		/// <summary>
		/// Gets an item.
		/// </summary>
		/// <returns>The found item</returns>
		/// <param name="item">Item to find</param>
		public DynamoDBItem GetItem(DynamoDBItem item)
		{
			if (item.HasHashKey)
			{
				if (_cachedItems.Exists(x => x.HashKey == item.HashKey))
				{
					return _cachedItems.Find (x => x.HashKey == item.HashKey);
				}
				else
				{
					DynamoDBItem gotItem = _table.GetItem(item.HashKey);
					_cachedItems.Add(gotItem);
					return gotItem;
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Puts an item in the table
		/// </summary>
		/// <param name="item">The item to put in the table</param>
		public void PutItem(DynamoDBItem item)
		{
			_table.PutItem(item);
			_cachedItems.Add(item);
		}

		/// <summary>
		/// Deletes an item from the table
		/// </summary>
		/// <param name="itemPrimaryKey">The value of the item's primary key</param>
		public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
		{
			_table.DeleteItem(itemPrimaryKey);
			_cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
		}
	}
}
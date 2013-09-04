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
        #region Methods

        #region GetItem
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
        #endregion

        #region PutItem
        /// <summary>
		/// Puts an item in the table
		/// </summary>
		/// <param name="item">The item to put in the table</param>
		public void PutItem(DynamoDBItem item)
		{
			_table.PutItem(item);
			_cachedItems.Add(item);
		}
        #endregion

        #region DeleteItem
        /// <summary>
		/// Deletes an item from the table
		/// </summary>
		/// <param name="itemPrimaryKey">The value of the item's primary key</param>
		public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
		{
			_table.DeleteItem(itemPrimaryKey);
			_cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
		}
        #endregion

        #endregion
    }
}
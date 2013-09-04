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

        #region GetItemAsync
        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <returns>The found item</returns>
        /// <param name="item">Item to find</param>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBItem item)
        {
            if (item.HasHashKey)
            {
                if (_cachedItems.Exists(x => x.HashKey == item.HashKey))
                {
                    return _cachedItems.Find(x => x.HashKey == item.HashKey);
                }
                else
                {
                    DynamoDBItem gotItem = await _table.GetItemAsync(item.HashKey);
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

        #region PutItemAsync
        /// <summary>
        /// Puts an item in the table
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public void PutItemAsync(DynamoDBItem item)
        {
            _table.PutItemAsync(item);
            _cachedItems.Add(item);
        }
        #endregion

        #region DeleteItemAsync
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public void DeleteItemAsync(DynamoDBAttribute itemPrimaryKey)
        {
            _table.DeleteItemAsync(itemPrimaryKey);
            _cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
        }
        #endregion

        #endregion
    }
}
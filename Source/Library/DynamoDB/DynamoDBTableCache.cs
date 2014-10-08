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
	public class DynamoDBTableCache
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
		public DynamoDBTableCache(AWSCredentials awsCredentials, String tableName, UInt64 cacheSize = 104857600)
		{
			try
			{
                _table = new DynamoDBTable(awsCredentials, tableName);
			}
			catch (Exception error)
			{
                throw DynamoDBTableCacheException.Generator(error);
			}
		}
        #endregion

        #region Methods

        #region Normal
#if !WINDOWSPHONE && !WINRT
        #region GetItem
        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <returns>The found item</returns>
        /// <param name="item">Item to find</param>
        public DynamoDBItem GetItem(DynamoDBItem item)
        {
			try
			{
	            if (item.HasHashKey)
	            {
	                if (_cachedItems.Exists(x => x.HashKey == item.HashKey))
	                {
	                    return _cachedItems.Find(x => x.HashKey == item.HashKey);
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
					throw DynamoDBTableCacheException.Generator(DynamoDBTableCacheException.Phases.DynamoDBItemHasNoHashKey);
	            }
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
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
			try
			{
	            _table.PutItem(item);
	            _cachedItems.Add(item);
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
			}
        }
        #endregion

        #region DeleteItem
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
	            _table.DeleteItem(itemPrimaryKey);
	            _cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
			}
        }
        #endregion
#endif
        #endregion

        #region Async
#if NET45
        #region GetItemAsync
        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <returns>The found item</returns>
        /// <param name="item">Item to find</param>
        public async Task<DynamoDBItem> GetItemAsync(DynamoDBItem item)
        {
			try
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
					throw DynamoDBTableCacheException.Generator(DynamoDBTableCacheException.Phases.DynamoDBItemHasNoHashKey);
	            }
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
			}
        }
        #endregion

        #region PutItemAsync
        /// <summary>
        /// Puts an item in the table
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public async Task PutItemAsync(DynamoDBItem item)
        {
			try
			{
	            await _table.PutItemAsync(item);
	            _cachedItems.Add(item);
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
			}
        }
        #endregion

        #region DeleteItemAsync
        /// <summary>
        /// Deletes an item from the table
        /// </summary>
        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
        public async Task DeleteItemAsync(DynamoDBAttribute itemPrimaryKey)
        {
			try
			{
            	await _table.DeleteItemAsync(itemPrimaryKey);
            	_cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
			}
			catch (Exception error)
			{			
				throw DynamoDBTableCacheException.Generator(error);
			}
        }
        #endregion
#endif
        #endregion

        #endregion
    }
}
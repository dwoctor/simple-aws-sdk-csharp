//using AWS.DynamoDB;
//using AWS.DynamoDB.Exceptions;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace AWS.DynamoDB
//{
//    /// <summary>
//    /// A DynamoDBTableCache
//    /// </summary>
//    public class DynamoDBTableCacheold
//    {
//        #region Fields
//        DynamoDBTable _table;
//        List<DynamoDBItem> _cachedItems = new List<DynamoDBItem>();
//        #endregion

//        #region Constructor
//        /// <summary>
//        /// Constructs a DynamoDBTableCache
//        /// </summary>
//        /// <param name="cacheSize">The size of the Cache in bytes (default value is 104857600 bytes or 100 mb)</param>
//        public DynamoDBTableCacheold(String accessKey, String secretAccessKey, String tableName, UInt64 cacheSize = 104857600)
//        {
//            _table = new DynamoDBTable(accessKey, secretAccessKey, tableName);
//        }
//        #endregion

//        #region Methods

//        #region GetItem
//        /// <summary>
//        /// Gets an item.
//        /// </summary>
//        /// <returns>The found item</returns>
//        /// <param name="item">Item to find</param>
//        public DynamoDBItem GetItem(DynamoDBItem item)
//        {
//            if (item.HasHashKey)
//            {
//                if (_cachedItems.Exists(x => x.HashKey == item.HashKey))
//                {
//                    return _cachedItems.Find (x => x.HashKey == item.HashKey);
//                }
//                else
//                {
//                    DynamoDBItem gotItem = _table.GetItem(item.HashKey);
//                    _cachedItems.Add(gotItem);
//                    return gotItem;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        /// <summary>
//        /// Gets an item.
//        /// </summary>
//        /// <returns>The found item</returns>
//        /// <param name="item">Item to find</param>
//        public async Task<DynamoDBItem> GetItemAsync(DynamoDBItem item)
//        {
//            if (item.HasHashKey)
//            {
//                if (_cachedItems.Exists(x => x.HashKey == item.HashKey))
//                {
//                    return _cachedItems.Find(x => x.HashKey == item.HashKey);
//                }
//                else
//                {
//                    DynamoDBItem gotItem = await _table.GetItemAsync(item.HashKey);
//                    _cachedItems.Add(gotItem);
//                    return gotItem;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }
//        #endregion

//        #region PutItem
//        /// <summary>
//        /// Puts an item in the table
//        /// </summary>
//        /// <param name="item">The item to put in the table</param>
//        public void PutItem(DynamoDBItem item)
//        {
//            _table.PutItem(item);
//            _cachedItems.Add(item);
//        }

//        /// <summary>
//        /// Puts an item in the table
//        /// </summary>
//        /// <param name="item">The item to put in the table</param>
//        public void PutItemAsync(DynamoDBItem item)
//        {
//            _table.PutItemAsync(item);
//            _cachedItems.Add(item);
//        }
//        #endregion

//        #region DeleteItem
//        /// <summary>
//        /// Deletes an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        public void DeleteItem(DynamoDBAttribute itemPrimaryKey)
//        {
//            _table.DeleteItem(itemPrimaryKey);
//            _cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
//        }
//        /// <summary>
//        /// Deletes an item from the table
//        /// </summary>
//        /// <param name="itemPrimaryKey">The value of the item's primary key</param>
//        public void DeleteItemAsync(DynamoDBAttribute itemPrimaryKey)
//        {
//            _table.DeleteItemAsync(itemPrimaryKey);
//            _cachedItems.RemoveAll(x => x.HashKey == itemPrimaryKey);
//        }
//        #endregion

//        #endregion
//    }
//}
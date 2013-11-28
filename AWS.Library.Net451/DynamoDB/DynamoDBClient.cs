using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBClient
    /// </summary>
    public class DynamoDBClient
    {
        #region Fields
        /// <summary>
        /// Stores the AWS access key
        /// </summary>
        private String _accessKey;

        /// <summary>
        /// Stores the AWS secret access key
        /// </summary>
        private String _secretAccessKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a DynamoDBClient
        /// </summary>
        public DynamoDBClient(String accessKey, String secretAccessKey)
        {
			try
			{
	            _accessKey = accessKey;
	            _secretAccessKey = secretAccessKey;
	            Tables = new List<DynamoDBTable>();
#if NET45
				var task = Task.Run(async () =>
				{
                    await GetTablesAsync();
				});
				task.Wait();
#else
				GetTables();
#endif
            }
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a table
        /// </summary>
        /// <param name="name">The name of the table to get</param>
        /// <returns>DynamoDBTable</returns>
        [System.Runtime.CompilerServices.IndexerName("DynamoDBClientIndex")]
        public DynamoDBTable this[String name]
        {
            get
            {
				try
				{
                	return Tables.Single(x => x.Name == name);
				}
				catch(Exception error)
				{
					throw DynamoDBClientException.Generator(error);
				}
            }
        }

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a table associated with an AWS account
        /// </summary>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="hashKeyAttributeName">The name of the hash key</param>
        /// <param name="hashKeyAttributeType">The type of the hash key</param>
        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
        public DynamoDBTable CreateTable(String tableName, String hashKeyAttributeName, Types.Enum hashKeyAttributeType, Int64 readCapacityUnits, Int64 writeCapacityUnits)
        {
			try
			{
	            DynamoDBTable table = DynamoDBTable.CreateTable(_accessKey, _secretAccessKey, tableName, hashKeyAttributeName, hashKeyAttributeType, readCapacityUnits, writeCapacityUnits);
	            Tables.Add(table);
	            Tables = Tables.OrderBy(x => x.Name).ToList();
	            return table;
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="tableName">The name of the table to delete</param>
        public void DeleteTable(String tableName)
        {
			try
			{
	            DynamoDBTable.DeleteTable(_accessKey, _secretAccessKey, tableName);
	            Tables.RemoveAll(x => x.Name == tableName);
	            Tables = Tables.OrderBy(x => x.Name).ToList();
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }

        /// <summary>
        /// Gets all the tables
        /// </summary>
        public void GetTables()
        {
			try
			{
	            List<DynamoDBTable> tables = DynamoDBTable.GetListOfTables(_accessKey, _secretAccessKey);
	            foreach (var table in tables)
	            {
	                if (Tables.Any(x => x.Name == table.Name) == false)
	                {
	                    Tables.Add(table);
	                }
	            }
	            foreach (var table in Tables)
	            {
	                if (tables.Any(x => x.Name == table.Name) == false)
	                {
	                    Tables.RemoveAll(x => x.Name == table.Name);
	                }
	            }
	            Tables = Tables.OrderBy(x => x.Name).ToList();
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }
#endif
        #endregion

        #region Async
#if NET45
        /// <summary>
        /// Creates a table associated with an AWS account
        /// </summary>
        /// <param name="tableName">The name of the table to create</param>
        /// <param name="hashKeyAttributeName">The name of the hash key</param>
        /// <param name="hashKeyAttributeType">The type of the hash key</param>
        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
        public async Task<DynamoDBTable> CreateTableAsync(String tableName, String hashKeyAttributeName, Types.Enum hashKeyAttributeType, Int64 readCapacityUnits, Int64 writeCapacityUnits)
        {
			try
			{
	            DynamoDBTable table = await DynamoDBTable.CreateTableAsync(_accessKey, _secretAccessKey, tableName, hashKeyAttributeName, hashKeyAttributeType, readCapacityUnits, writeCapacityUnits);
	            Tables.Add(table);
	            Tables = Tables.OrderBy(x => x.Name).ToList();
	            return table;
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="tableName">The name of the table to delete</param>
        public void DeleteTableAsync(String tableName)
        {
			try
			{
	            DynamoDBTable.DeleteTableAsync(_accessKey, _secretAccessKey, tableName);
	            Tables.RemoveAll(x => x.Name == tableName);
	            Tables = Tables.OrderBy(x => x.Name).ToList();
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }

        /// <summary>
        /// Gets all the tables
        /// </summary>
        public async Task GetTablesAsync()
        {
			try
			{
	            List<DynamoDBTable> tables = await DynamoDBTable.GetListOfTablesAsync(_accessKey, _secretAccessKey);
	            foreach (var table in tables)
	            {
	                if (Tables.Any(x => x.Name == table.Name) == false)
	                {
	                    Tables.Add(table);
	                }
	            }
	            foreach (var table in Tables)
	            {
	                if (tables.Any(x => x.Name == table.Name) == false)
	                {
	                    Tables.RemoveAll(x => x.Name == table.Name);
	                }
	            }
	            Tables = Tables.OrderBy(x => x.Name).ToList();
			}
			catch(Exception error)
			{
				throw DynamoDBClientException.Generator(error);
			}
        }
#endif
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// The tables
        /// </summary>
        public List<DynamoDBTable> Tables { get; private set; }
        #endregion
    }
}
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
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
    public partial class DynamoDBClient
    {
        #region Methods
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
            DynamoDBTable table = await DynamoDBTable.CreateTableAsync(_accessKey, _secretAccessKey, tableName, hashKeyAttributeName, hashKeyAttributeType, readCapacityUnits, writeCapacityUnits);
            Tables.Add(table);
            Tables = Tables.OrderBy(x => x.Name).ToList();
            return table;
        }

        /// <summary>
        /// Delete a table associated with an AWS account
        /// </summary>
        /// <param name="tableName">The name of the table to delete</param>
        public void DeleteTableAsync(String tableName)
        {
            DynamoDBTable.DeleteTableAsync(_accessKey, _secretAccessKey, tableName);
            Tables.RemoveAll(x => x.Name == tableName);
            Tables = Tables.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets all the tables
        /// </summary>
        public async void GetTablesAsync()
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
        #endregion
    }
}
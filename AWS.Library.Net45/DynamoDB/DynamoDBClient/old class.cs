//using Amazon.DynamoDBv2;
//using Amazon.DynamoDBv2.Model;
//using AWS.DynamoDB;
//using AWS.DynamoDB.Exceptions;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace AWS.DynamoDB
//{
//    /// <summary>
//    /// A DynamoDBClient
//    /// </summary>
//    public class DynamoDBClient
//    {
//        #region Fields
//        /// <summary>
//        /// Stores the AWS access key
//        /// </summary>
//        private String _accessKey;

//        /// <summary>
//        /// Stores the AWS secret access key
//        /// </summary>
//        private String _secretAccessKey;
//        #endregion

//        #region Constructors
//        /// <summary>
//        /// Constructs a DynamoDBClient
//        /// </summary>
//        public DynamoDBClient(String accessKey, String secretAccessKey)
//        {
//            _accessKey = accessKey;
//            _secretAccessKey = secretAccessKey;
//            Tables = new List<DynamoDBTable>();
//            GetTables();
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Creates a table associated with an AWS account
//        /// </summary>
//        /// <param name="tableName">The name of the table to create</param>
//        /// <param name="hashKeyAttributeName">The name of the hash key</param>
//        /// <param name="hashKeyAttributeType">The type of the hash key</param>
//        /// <param name="readCapacityUnits">The number read capacity units provisioned for the database</param>
//        /// <param name="writeCapacityUnits">The number write capacity units provisioned for the database</param>
//        public DynamoDBTable CreateTable(String tableName, String hashKeyAttributeName, Types.Enum hashKeyAttributeType, Int64 readCapacityUnits, Int64 writeCapacityUnits)
//        {
//            DynamoDBTable table = DynamoDBTable.CreateTable(_accessKey, _secretAccessKey, tableName, hashKeyAttributeName, hashKeyAttributeType, readCapacityUnits, writeCapacityUnits);
//            Tables.Add(table);
//            Tables = Tables.OrderBy(x => x.Name).ToList();
//            return table;
//        }

//        /// <summary>
//        /// Delete a table associated with an AWS account
//        /// </summary>
//        /// <param name="tableName">The name of the table to delete</param>
//        public void DeleteTable(String tableName)
//        {
//            DynamoDBTable.DeleteTable(_accessKey, _secretAccessKey, tableName);
//            Tables.RemoveAll(x => x.Name == tableName);
//            Tables = Tables.OrderBy(x => x.Name).ToList();
//        }

//        /// <summary>
//        /// Gets all the tables
//        /// </summary>
//        public void GetTables()
//        {
//            List<DynamoDBTable> tables = DynamoDBTable.GetListOfTables(_accessKey, _secretAccessKey);
//            foreach (var table in tables)
//            {
//                if (Tables.Any(x => x.Name == table.Name) == false)
//                {
//                    Tables.Add(table);
//                }
//            }
//            foreach (var table in Tables)
//            {
//                if (tables.Any(x => x.Name == table.Name) == false)
//                {
//                    Tables.RemoveAll(x => x.Name == table.Name);
//                }
//            }
//            Tables = Tables.OrderBy(x => x.Name).ToList();
//        }

//        /// <summary>
//        /// Get a table
//        /// </summary>
//        /// <param name="name">The name of the table to get</param>
//        /// <returns>DynamoDBTable</returns>
//        [System.Runtime.CompilerServices.IndexerName("DynamoDBClientIndex")]
//        public DynamoDBTable this[String name]
//        {
//            get
//            {
//                return Tables.Single(x => x.Name == name);
//            }
//        }
//        #endregion

//        #region Properties
//        /// <summary>
//        /// The tables
//        /// </summary>
//        public List<DynamoDBTable> Tables { get; private set; }
//        #endregion
//    }
//}
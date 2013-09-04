using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBClient
    /// </summary>
    public partial class DynamoDBClient
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
            _accessKey = accessKey;
            _secretAccessKey = secretAccessKey;
            Tables = new List<DynamoDBTable>();
            //GetTables();
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
                return Tables.Single(x => x.Name == name);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The tables
        /// </summary>
        public List<DynamoDBTable> Tables { get; private set; }
        #endregion
    }
}
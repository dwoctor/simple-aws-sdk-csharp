using System;
using System.Collections.Generic;
using AWS;
using AWS.DynamoDB;
using AWS.Tests;

namespace AWS.Tests.DynamoDB
{
    /// <summary>
    /// DynamoDBTestData
    /// </summary>
    public class DynamoDBTestData : TestData
    {

        #region Fields
        /// <summary>
        /// Stores the TableName
        /// </summary>
        private String _tableName;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a DynamoDBTestData object
        /// </summary>
        public DynamoDBTestData() : base()
        {
            GenerateNewTableName();
            this.HashKeyName = "TestHashKey";
            this.ReadCapacityUnits = 1;
            this.WriteCapacityUnits = 1;
            this.HashKeyValue = 1;
            this.StringAttributeKey = "String";
            this.NumberAttributeKey = "Number";
            this.BinaryAttributeKey = "Binary";
            this.StringAttributeValue = "Text";
            this.NumberAttributeValue = new Random().Next();
            this.BinaryAttributeValue = new Byte[] { Byte.MinValue, Byte.MaxValue };
            this.HashKeyAttribute = new DynamoDBAttribute(this.HashKeyName, this.HashKeyValue, true);
            this.HashKeyAttributeNoValue = new DynamoDBAttribute(this.HashKeyName, Types.Enum.Number, true);
            this.StringAttribute = new DynamoDBAttribute(this.StringAttributeKey, this.StringAttributeValue);
            this.NumberAttribute = new DynamoDBAttribute(this.NumberAttributeKey, this.NumberAttributeValue);
            this.BinaryAttribute = new DynamoDBAttribute(this.BinaryAttributeKey, this.BinaryAttributeValue);
            this.DDBItem = new DynamoDBItem(new List<DynamoDBAttribute>() { this.HashKeyAttribute, this.StringAttribute, this.NumberAttribute, this.BinaryAttribute });
            this.DDBClient = new DynamoDBClient(this.Credentials);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get and Set the TableName
        /// </summary>
        public String TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
                GenerateNewDDBTableAndDDBTableCache();
            }
        }

        /// <summary>
        /// Get and Set the HashKeyName
        /// </summary>
        public String HashKeyName { get; set; }

        /// <summary>
        /// Get and Set the ReadCapacityUnits
        /// </summary>
        public Int64 ReadCapacityUnits { get; set; }

        /// <summary>
        /// Get and Set the WriteCapacityUnits
        /// </summary>
        public Int64 WriteCapacityUnits { get; set; }

        /// <summary>
        /// Get and Set the HashKeyValue
        /// </summary>
        public Int32 HashKeyValue { get; set; }

        /// <summary>
        /// Get and Set the StringAttributeKey
        /// </summary>
        public String StringAttributeKey { get; set; }

        /// <summary>
        /// Get and Set the NumberAttributeKey
        /// </summary>
        public String NumberAttributeKey { get; set; }

        /// <summary>
        /// Get and Set the BinaryAttributeKey
        /// </summary>
        public String BinaryAttributeKey { get; set; }

        /// <summary>
        /// Get and Set the StringAttributeValue
        /// </summary>
        public String StringAttributeValue { get; set; }

        /// <summary>
        /// Get and Set the NumberAttributeValue
        /// </summary>
        public Int32 NumberAttributeValue { get; set; }

        /// <summary>
        /// Get and Set the BinaryAttributeValue
        /// </summary>
        public Byte[] BinaryAttributeValue { get; set; }

        /// <summary>
        /// Get and Set the HashKeyAttribute
        /// </summary>
        public DynamoDBAttribute HashKeyAttribute { get; set; }

        /// <summary>
        /// Get and Set the HashKeyAttributeNoValue
        /// </summary>
        public DynamoDBAttribute HashKeyAttributeNoValue { get; set; }

        /// <summary>
        /// Get and Set the StringAttribute
        /// </summary>
        public DynamoDBAttribute StringAttribute { get; set; }

        /// <summary>
        /// Get and Set the NumberAttribute
        /// </summary>
        public DynamoDBAttribute NumberAttribute { get; set; }

        /// <summary>
        /// Get and Set the BinaryAttribute
        /// </summary>
        public DynamoDBAttribute BinaryAttribute { get; set; }

        /// <summary>
        /// Get and Set the DDBItem
        /// </summary>
        public DynamoDBItem DDBItem { get; set; }

        /// <summary>
        /// Get and Set the DDBTable
        /// </summary>
        public DynamoDBTable DDBTable { get; set; }

        /// <summary>
        /// Get and Set the DDBTableCache
        /// </summary>
        public DynamoDBTableCache DDBTableCache { get; set; }

        /// <summary>
        /// Get and Set the DDBClient
        /// </summary>
        public DynamoDBClient DDBClient { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new TableName
        /// </summary>
        /// <remarks>
        /// TableName is a concatenation of various elements.
        /// 
        /// TableName string format: {0}-{1}-{2}{3}{4}{5}{6}{7}{8}{9}
        /// 
        /// TableName string data:
        /// 
        /// {0} TestTable
        /// {1} Random Int32
        /// {2} DateTime.UtcNow.Year
        /// {3} DateTime.UtcNow.Month
        /// {4} DateTime.UtcNow.Day
        /// {5} DateTime.UtcNow.Hour
        /// {6} DateTime.UtcNow.Minute
        /// {7} DateTime.UtcNow.Second
        /// {8} DateTime.UtcNow.Millisecond
        /// {9} DateTime.UtcNow.Ticks
        /// </remarks>
        public void GenerateNewTableName()
        {
            DateTime time = DateTime.UtcNow;
            Int32 testnumber = new Random().Next();
            String tableName = String.Format("{0}-{1}-{2}{3}{4}{5}{6}{7}{8}{9}", "TestTable", testnumber, time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond, time.Ticks);
            this.TableName = tableName;
            GenerateNewDDBTableAndDDBTableCache();
        }

        /// <summary>
        /// Creates a new DDBTable and DDBTableCache Objects
        /// </summary>
        public void GenerateNewDDBTableAndDDBTableCache()
        {
            this.DDBTable = new DynamoDBTable(this.Credentials, this.TableName);
            this.DDBTableCache = new DynamoDBTableCache(this.Credentials, this.TableName);
        }
        #endregion

    }
}
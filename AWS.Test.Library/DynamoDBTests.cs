using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;

namespace AWS.Test.Library
{
	public class DynamoDBTests
	{
		protected static String _tableName = "TestTable";
		protected static String _hashKeyName = "TestHashKey";
		protected static Int64 _readCapacityUnits = 1;
		protected static Int64 _writeCapacityUnits = 1;
		protected static Int32 _hashKeyValue = 1;
		protected static String _stringAttributeKey = "String";
		protected static String _numberAttributeKey = "Number";
		protected static String _binaryAttributeKey = "Binary";
		protected static String _stringAttributeValue = "Text";
		protected static Int32 _numberAttributeValue = new Random().Next();
		protected static Byte[] _binaryAttributeValue = new Byte[] { Byte.MinValue, Byte.MaxValue };
		protected static DynamoDBAttribute _hashKeyAttribute = new DynamoDBAttribute(_hashKeyName, _hashKeyValue, true);
		protected static DynamoDBAttribute _stringAttribute = new DynamoDBAttribute(_stringAttributeKey, _stringAttributeValue);
		protected static DynamoDBAttribute _numberAttribute = new DynamoDBAttribute(_numberAttributeKey, _numberAttributeValue);
		protected static DynamoDBAttribute _binaryAttribute = new DynamoDBAttribute(_binaryAttributeKey, _binaryAttributeValue);
		protected static DynamoDBItem _item = new DynamoDBItem(new List<DynamoDBAttribute>() { _hashKeyAttribute, _stringAttribute, _numberAttribute, _binaryAttribute });
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.DynamoDB.Exceptions
{
    internal class DynamoDBTableExceptions
    {
        public static Exception TableAlreadyExists(String tableName)
        {
            throw new DynamoDBTableException(String.Format("Table '{0}' Already Exists", tableName));
        }

        public static Exception TableDoesNotExist(String tableName)
        {
            throw new DynamoDBTableException(String.Format("Table '{0}' does not Exist", tableName));
        }

        public static Exception DynamoDBItemContainsNoHashKey()
        {
            throw new DynamoDBTableException("DynamoDBItem contains no Hash Key");
        }

        public static Exception DynamoDBAttributeContainsNoHashKey()
        {
            throw new DynamoDBTableException("DynamoDBAttribute contains no Hash Key");
        }

        public static Exception UnsupportedType()
        {
            throw new DynamoDBTableException("Unsupported Type");
        }
    }
}
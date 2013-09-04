using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.DynamoDB.Exceptions
{
    internal class DynamoDBItemExceptions
    {
        public static Exception ContainsNoAttributes()
        {
            throw new DynamoDBTableException("Contains No Attributes");
        }
    }
}
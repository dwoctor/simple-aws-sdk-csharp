using System;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
    /// A DynamoDBTable Exception
    /// </summary>
    //[Serializable]
    public class DynamoDBTableException : Exception
    {
        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBTableException() : base() { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBTableException(string message) : base(message) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBTableException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBTableException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBTableException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a DynamoDBTableException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected DynamoDBTableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
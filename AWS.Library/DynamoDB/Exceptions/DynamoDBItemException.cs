using System;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
    /// A DynamoDBItem Exception
    /// </summary>
    //[Serializable]
    public class DynamoDBItemException : Exception
    {
        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBItemException() : base() { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBItemException(string message) : base(message) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBItemException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBItemException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a DynamoDBTableException
        /// </summary>
        public DynamoDBItemException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a DynamoDBTableException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected DynamoDBItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
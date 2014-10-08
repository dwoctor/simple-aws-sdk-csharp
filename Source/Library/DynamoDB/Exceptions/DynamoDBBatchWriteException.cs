using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
	/// A DynamoDBBatchWriteException
    /// </summary>
    //[Serializable]
	public class DynamoDBBatchWriteException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("DynamoDBBatchWrite: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new DynamoDBBatchWriteException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is DynamoDBBatchWriteException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBBatchWriteException
        /// </summary>
		internal DynamoDBBatchWriteException() : base() { }

        /// <summary>
		/// Constructs a DynamoDBBatchWriteException
        /// </summary>
		internal DynamoDBBatchWriteException(string message) : base(message) { }

        /// <summary>
		/// Constructs a DynamoDBBatchWriteException
        /// </summary>
		internal DynamoDBBatchWriteException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
		/// Constructs a DynamoDBBatchWriteException
        /// </summary>
		internal DynamoDBBatchWriteException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
		/// Constructs a DynamoDBBatchWriteException
        /// </summary>
		internal DynamoDBBatchWriteException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
		///// Constructs a DynamoDBBatchWriteException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
		//protected DynamoDBBatchWriteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
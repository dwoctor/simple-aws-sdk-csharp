using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
	/// A DynamoDBClientException
    /// </summary>
    //[Serializable]
	public class DynamoDBClientException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("DynamoDBClient: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new DynamoDBClientException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is DynamoDBClientException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBClientException
        /// </summary>
		internal DynamoDBClientException() : base() { }

        /// <summary>
		/// Constructs a DynamoDBClientException
        /// </summary>
		internal DynamoDBClientException(string message) : base(message) { }

        /// <summary>
		/// Constructs a DynamoDBClientException
        /// </summary>
		internal DynamoDBClientException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
		/// Constructs a DynamoDBClientException
        /// </summary>
		internal DynamoDBClientException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
		/// Constructs a DynamoDBClientException
        /// </summary>
		internal DynamoDBClientException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
		///// Constructs a DynamoDBClientException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
		//protected DynamoDBClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
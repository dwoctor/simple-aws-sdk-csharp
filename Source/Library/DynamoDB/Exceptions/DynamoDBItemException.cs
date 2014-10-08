using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
	/// A DynamoDBItemException
    /// </summary>
    //[Serializable]
	public class DynamoDBItemException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("DynamoDBItem: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new DynamoDBItemException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is DynamoDBItemException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBItemException
        /// </summary>
		internal DynamoDBItemException() : base() { }

        /// <summary>
		/// Constructs a DynamoDBItemException
        /// </summary>
		internal DynamoDBItemException(string message) : base(message) { }

        /// <summary>
		/// Constructs a DynamoDBItemException
        /// </summary>
		internal DynamoDBItemException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
		/// Constructs a DynamoDBItemException
        /// </summary>
		internal DynamoDBItemException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
		/// Constructs a DynamoDBItemException
        /// </summary>
		internal DynamoDBItemException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
		///// Constructs a DynamoDBItemException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
		//protected DynamoDBItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
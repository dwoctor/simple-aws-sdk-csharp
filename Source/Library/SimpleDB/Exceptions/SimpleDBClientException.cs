using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SimpleDB.Exceptions
{
    /// <summary>
    /// A SimpleDBClientException
    /// </summary>
    //[Serializable]
    public class SimpleDBClientException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SimpleDBClient: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SimpleDBClientException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SimpleDBClientException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBClientException
        /// </summary>
        public SimpleDBClientException() : base() { }

        /// <summary>
        /// Constructs a SimpleDBClientException
        /// </summary>
        public SimpleDBClientException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SimpleDBClientException
        /// </summary>
        public SimpleDBClientException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SimpleDBClientException
        /// </summary>
        public SimpleDBClientException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SimpleDBClientException
        /// </summary>
        public SimpleDBClientException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SimpleDBClientException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SimpleDBClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
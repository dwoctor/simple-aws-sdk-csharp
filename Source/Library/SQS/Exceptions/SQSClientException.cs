using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SQS.Exceptions
{
    /// <summary>
    /// A SQSClientException
    /// </summary>
    //[Serializable]
    public class SQSClientException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SQSClient: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SQSClientException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SQSClientException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SQSClientException
        /// </summary>
        public SQSClientException() : base() { }

        /// <summary>
        /// Constructs a SQSClientException
        /// </summary>
        public SQSClientException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SQSClientException
        /// </summary>
        public SQSClientException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SQSClientException
        /// </summary>
        public SQSClientException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SQSClientException
        /// </summary>
        public SQSClientException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SQSClientException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SQSClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
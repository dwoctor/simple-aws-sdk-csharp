using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SQS.Exceptions
{
    /// <summary>
    /// A SQSMessageException
    /// </summary>
    //[Serializable]
    public class SQSMessageException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SQSMessage: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SQSMessageException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SQSMessageException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SQSMessageException
        /// </summary>
        public SQSMessageException() : base() { }

        /// <summary>
        /// Constructs a SQSMessageException
        /// </summary>
        public SQSMessageException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SQSMessageException
        /// </summary>
        public SQSMessageException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SQSMessageException
        /// </summary>
        public SQSMessageException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SQSMessageException
        /// </summary>
        public SQSMessageException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SQSMessageException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SQSMessageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
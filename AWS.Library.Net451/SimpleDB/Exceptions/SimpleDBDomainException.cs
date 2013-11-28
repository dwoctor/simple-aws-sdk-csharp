using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SimpleDB.Exceptions
{
    /// <summary>
    /// A SimpleDBDomainException
    /// </summary>
    //[Serializable]
    public class SimpleDBDomainException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SimpleDBDomain: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SimpleDBDomainException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SimpleDBDomainException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBDomainException
        /// </summary>
        public SimpleDBDomainException() : base() { }

        /// <summary>
        /// Constructs a SimpleDBDomainException
        /// </summary>
        public SimpleDBDomainException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SimpleDBDomainException
        /// </summary>
        public SimpleDBDomainException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SimpleDBDomainException
        /// </summary>
        public SimpleDBDomainException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SimpleDBDomainException
        /// </summary>
        public SimpleDBDomainException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SimpleDBDomainException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SimpleDBDomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
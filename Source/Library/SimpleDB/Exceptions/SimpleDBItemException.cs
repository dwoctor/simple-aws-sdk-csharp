using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SimpleDB.Exceptions
{
    /// <summary>
    /// A SimpleDBItemException
    /// </summary>
    //[Serializable]
    public class SimpleDBItemException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SimpleDBItem: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SimpleDBItemException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SimpleDBItemException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItemException
        /// </summary>
        public SimpleDBItemException() : base() { }

        /// <summary>
        /// Constructs a SimpleDBItemException
        /// </summary>
        public SimpleDBItemException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SimpleDBItemException
        /// </summary>
        public SimpleDBItemException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SimpleDBItemException
        /// </summary>
        public SimpleDBItemException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SimpleDBItemException
        /// </summary>
        public SimpleDBItemException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SimpleDBItemException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SimpleDBItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SimpleDB.Exceptions
{
    /// <summary>
    /// A SimpleDBAttributeException
    /// </summary>
    //[Serializable]
    public class SimpleDBAttributeException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SimpleDBAttribute: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SimpleDBAttributeException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SimpleDBAttributeException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBAttributeException
        /// </summary>
        public SimpleDBAttributeException() : base() { }

        /// <summary>
        /// Constructs a SimpleDBAttributeException
        /// </summary>
        public SimpleDBAttributeException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SimpleDBAttributeException
        /// </summary>
        public SimpleDBAttributeException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SimpleDBAttributeException
        /// </summary>
        public SimpleDBAttributeException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SimpleDBAttributeException
        /// </summary>
        public SimpleDBAttributeException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SimpleDBAttributeException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SimpleDBAttributeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
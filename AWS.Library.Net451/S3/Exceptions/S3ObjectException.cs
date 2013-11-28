using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.S3.Exceptions
{
    /// <summary>
    /// A S3ObjectException
    /// </summary>
    //[Serializable]
    public class S3ObjectException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("S3Object: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new S3ObjectException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is S3ObjectException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a S3ObjectException
        /// </summary>
        public S3ObjectException() : base() { }

        /// <summary>
        /// Constructs a S3ObjectException
        /// </summary>
        public S3ObjectException(string message) : base(message) { }

        /// <summary>
        /// Constructs a S3ObjectException
        /// </summary>
        public S3ObjectException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a S3ObjectException
        /// </summary>
        public S3ObjectException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a S3ObjectException
        /// </summary>
        public S3ObjectException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a S3ObjectException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected S3ObjectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
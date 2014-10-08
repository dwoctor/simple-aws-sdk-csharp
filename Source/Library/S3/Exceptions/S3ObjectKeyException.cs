using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.S3.Exceptions
{
    /// <summary>
    /// A S3ObjectKeyException
    /// </summary>
    //[Serializable]
    public class S3ObjectKeyException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("S3ObjectKey: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new S3ObjectKeyException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is S3ObjectKeyException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a S3ObjectKeyException
        /// </summary>
        public S3ObjectKeyException() : base() { }

        /// <summary>
        /// Constructs a S3ObjectKeyException
        /// </summary>
        public S3ObjectKeyException(string message) : base(message) { }

        /// <summary>
        /// Constructs a S3ObjectKeyException
        /// </summary>
        public S3ObjectKeyException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a S3ObjectKeyException
        /// </summary>
        public S3ObjectKeyException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a S3ObjectKeyException
        /// </summary>
        public S3ObjectKeyException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a S3ObjectKeyException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected S3ObjectKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
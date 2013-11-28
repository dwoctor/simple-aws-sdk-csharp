using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.S3.Exceptions
{
    /// <summary>
    /// A S3ClientException
    /// </summary>
    //[Serializable]
    public class S3ClientException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("S3Client: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new S3ClientException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is S3ClientException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a S3ClientException
        /// </summary>
        public S3ClientException() : base() { }

        /// <summary>
        /// Constructs a S3ClientException
        /// </summary>
        public S3ClientException(string message) : base(message) { }

        /// <summary>
        /// Constructs a S3ClientException
        /// </summary>
        public S3ClientException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a S3ClientException
        /// </summary>
        public S3ClientException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a S3ClientException
        /// </summary>
        public S3ClientException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a S3ClientException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected S3ClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.SQS.Exceptions
{
    /// <summary>
    /// A SQSQueueException
    /// </summary>
    //[Serializable]
    public class SQSQueueException : Exception
    {
        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("SQSQueue: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new SQSQueueException(errorMessage);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is SQSQueueException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
        /// Constructs a SQSQueueException
        /// </summary>
        public SQSQueueException() : base() { }

        /// <summary>
        /// Constructs a SQSQueueException
        /// </summary>
        public SQSQueueException(string message) : base(message) { }

        /// <summary>
        /// Constructs a SQSQueueException
        /// </summary>
        public SQSQueueException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Constructs a SQSQueueException
        /// </summary>
        public SQSQueueException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructs a SQSQueueException
        /// </summary>
        public SQSQueueException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
        ///// Constructs a SQSQueueException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
        //protected SQSQueueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
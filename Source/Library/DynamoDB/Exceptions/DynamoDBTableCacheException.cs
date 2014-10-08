using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
    /// A DynamoDBTable Exception
    /// </summary>
    //[Serializable]
	public class DynamoDBTableCacheException : Exception
    {
		internal sealed class Phases 
		{
			private readonly String name;
			private readonly Int32 value;

			public static readonly Phases DynamoDBItemHasNoHashKey = new Phases(1, "DynamoDBItem Has No Hash Key");      

			private Phases(Int32 value, String name)
			{
				this.name = name;
				this.value = value;
			}

			public override String ToString()
			{
				return name;
			}
		}

        internal static Exception Generator(String exceptionMessage, [CallerMemberName] String callerMemberName = "")
        {
            String errorMessage = String.Format("DynamoDBTableCache: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new DynamoDBTableCacheException(errorMessage);
        }

        internal static Exception Generator(Phases phase, [CallerMemberName] String callerMemberName = "")
        {
            throw Generator(phase.ToString());
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is DynamoDBTableCacheException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBTableCacheException
        /// </summary>
		internal DynamoDBTableCacheException() : base() { }

        /// <summary>
		/// Constructs a DynamoDBTableCacheException
        /// </summary>
		internal DynamoDBTableCacheException(string message) : base(message) { }

        /// <summary>
		/// Constructs a DynamoDBTableCacheException
        /// </summary>
		internal DynamoDBTableCacheException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
		/// Constructs a DynamoDBTableCacheException
        /// </summary>
		internal DynamoDBTableCacheException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
		/// Constructs a DynamoDBTableCacheException
        /// </summary>
		internal DynamoDBTableCacheException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
		///// Constructs a DynamoDBTableCacheException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
		//protected DynamoDBTableCacheException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AWS.DynamoDB.Exceptions
{
    /// <summary>
	/// A DynamoDBAttributeException
    /// </summary>
    //[Serializable]
	public class DynamoDBAttributeException : Exception
    {
		internal sealed class Phases 
		{
			private readonly String name;
			private readonly Int32 value;

			public static readonly Phases UnsupportedType = new Phases(1, "Unsupported Type");   

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
            String errorMessage = String.Format("DynamoDBAttribute: {0} (Error) {1}", callerMemberName, exceptionMessage);
            Debug.WriteLine(errorMessage);
            throw new DynamoDBAttributeException(errorMessage);
        }

        internal static Exception Generator(Phases phase, [CallerMemberName] String callerMemberName = "")
        {
            throw Generator(phase, callerMemberName);
        }

        internal static Exception Generator(Exception exception, [CallerMemberName] String callerMemberName = "")
        {
            if (exception is DynamoDBAttributeException)
            {
                throw exception;
            }
            else
            {
                throw Generator(exception.Message, callerMemberName);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBAttributeException
        /// </summary>
		internal DynamoDBAttributeException() : base() { }

        /// <summary>
		/// Constructs a DynamoDBAttributeException
        /// </summary>
		internal DynamoDBAttributeException(string message) : base(message) { }

        /// <summary>
		/// Constructs a DynamoDBAttributeException
        /// </summary>
		internal DynamoDBAttributeException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
		/// Constructs a DynamoDBAttributeException
        /// </summary>
		internal DynamoDBAttributeException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
		/// Constructs a DynamoDBAttributeException
        /// </summary>
		internal DynamoDBAttributeException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        ///// <summary>
		///// Constructs a DynamoDBAttributeException
        ///// </summary>
        ///// <remarks>
        ///// A constructor is needed for serialization when an 
        ///// exception propagates from a remoting server to the client. 
        ///// </remarks>
		//protected DynamoDBAttributeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
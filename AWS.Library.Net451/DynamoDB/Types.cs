using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;

namespace AWS.DynamoDB
{
    /// <summary>
    ///  Indicates the different types of data DynamoDB can store
    /// </summary>
    public class Types
    {
        /// <summary>
		/// The enums of the different types of data DynamoDB can store
		/// </summary>
		public enum Enum
		{
            /// <summary>
            /// A string type
            /// </summary>
			String,
            /// <summary>
            /// A number type
            /// </summary>
			Number,
            /// <summary>
            /// A binary type
            /// </summary>
			Binary
		};	

		/// <summary>
		/// The strings of the different types of data DynamoDB can store
		/// </summary>
        internal static readonly Dictionary<Enum, String> String = new Dictionary<Enum, String>()
		{
			{Enum.String, "S"},
			{Enum.Number, "N"},
			{Enum.Binary, "B"}
		};

        /// <summary>
        /// The strings of the different types of data DynamoDB can store
        /// </summary>
        internal static readonly Dictionary<Enum, ScalarAttributeType> ScalarAttributeType = new Dictionary<Enum, ScalarAttributeType>()
		{
			{Enum.String, new ScalarAttributeType("S")},
			{Enum.Number, new ScalarAttributeType("N")},
			{Enum.Binary, new ScalarAttributeType("B")}
		};
	}
}
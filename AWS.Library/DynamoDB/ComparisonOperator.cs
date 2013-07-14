using System;
using System.Collections.Generic;

namespace AWS.DynamoDB
{
    /// <summary>
    /// The DynamoDB Comparison Operator
    /// </summary>
	public class ComparisonOperator
	{
		/// <summary>
		/// Indicates the different comparison operators when updating an item
		/// </summary>
		public enum Enum
		{
            /// <summary>
            /// Equal Operator
            /// </summary>
			Equal,
            /// <summary>
            /// Not Equal Operator
            /// </summary>
			NotEqual,
            /// <summary>
            /// Less Than Or Equal Operator
            /// </summary>
			LessThanOrEqual,
            /// <summary>
            /// Less Than Operator
            /// </summary>
			LessThan,
            /// <summary>
            /// Greater Than Or Equal Operator
            /// </summary>
			GreaterThanOrEqual,
            /// <summary>
            /// Greater Than Operator
            /// </summary>
			GreaterThan,
            /// <summary>
            /// Attribute Exists Operator
            /// </summary>
			AttributeExists,
            /// <summary>
            /// Attribute Does Not Exist Operator
            /// </summary>
			AttributeDoesNotExist,
            /// <summary>
            /// Checks For A Subsequence Or Value In A Set Operator
            /// </summary>
			ChecksForASubsequenceOrValueInASet,
            /// <summary>
            /// Checks For Absence Of A Subsequence Or Absence Of A Value In A Set Operator
            /// </summary>
			ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet,
            /// <summary>
            /// Checks For A Prefix Operator
            /// </summary>
			ChecksForAPrefix,
            /// <summary>
            /// Checks For Exact Matches Operator
            /// </summary>
			ChecksForExactMatches,
            /// <summary>
            /// Greater Than Or Equal To The First Value And Less Than Or Equal To The Second Value Operator
            /// </summary>
			GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue
		};	

		/// <summary>
		/// The strings of the different comparison operators
		/// </summary>
        internal static readonly Dictionary<Enum, String> String = new Dictionary<Enum, String>()
		{
			{Enum.Equal, "EQ"},
			{Enum.NotEqual, "NE"},
			{Enum.LessThanOrEqual, "LE"},
			{Enum.LessThan, "LT"},
			{Enum.GreaterThanOrEqual, "GE"},
			{Enum.GreaterThan, "GT"},
			{Enum.AttributeExists, "NOT_NULL"},
			{Enum.AttributeDoesNotExist, "NULL"},
			{Enum.ChecksForASubsequenceOrValueInASet, "CONTAINS"},
			{Enum.ChecksForAbsenceOfASubsequenceOrAbsenceOfAValueInASet, "NOT_CONTAINS"},
			{Enum.ChecksForAPrefix, "BEGINS_WITH"},
			{Enum.ChecksForExactMatches, "IN"},
			{Enum.GreaterThanOrEqualToTheFirstValueAndLessThanOrEqualToTheSecondValue, "BETWEEN"}
		};
	}
}
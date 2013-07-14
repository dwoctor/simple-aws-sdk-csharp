using System;

namespace AWS.DynamoDB
{
	/// <summary>
	/// Indicates an Update Action
	/// </summary>
	public enum UpdateAction
	{
        /// <summary>
        /// PUT Update Action
        /// </summary>
		PUT,
        /// <summary>
        /// DELETE Update Action
        /// </summary>
		DELETE,
        /// <summary>
        /// ADD Update Action
        /// </summary>
		ADD
	};
}
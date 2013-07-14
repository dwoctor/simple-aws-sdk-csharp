using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Test.Library
{
    /// <summary>
    /// Stores the AWS Credentials
    /// </summary>
    class AWSCredentials
    {
        #region Fields
        /// <summary>
        /// Stores the AWS access key
        /// </summary>
		public static String AccessKey = "<Insert AWS Access Key Here>";

        /// <summary>
        /// Stores the AWS secret access key
        /// </summary>
		public static String SecretAccessKey = "<Insert AWS Secret Access Key Here>";
        #endregion
    }
}
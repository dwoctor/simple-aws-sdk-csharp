using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.S3
{
    /// <summary>
    /// A S3ObjectKey
    /// </summary>
    public class S3ObjectKey
    {
        #region Constructors
        /// <summary>
        /// Constructs an S3ObjectKey
        /// </summary>
        public S3ObjectKey(String key)
        {
            this.Value = key;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the Key of the S3ObjectKey
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            return Value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The S3ObjectKey's Value
        /// </summary>
        public String Value { get; set; }
        #endregion
    }
}
using AWS.S3;
using AWS.S3.Exceptions;
using System;

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
            try
            {
                this.Value = key;
            }
            catch (Exception error)
            {
                throw S3ObjectKeyException.Generator(error);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the Key of the S3ObjectKey
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            try
            {
                return Value;
            }
            catch (Exception error)
            {
                throw S3ObjectKeyException.Generator(error);
            }
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
using Amazon.S3;
//using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Model.Internal;
using Amazon.S3.Model.Internal.MarshallTransformations;
//using Amazon.S3.Transfer;
//using Amazon.S3.Transfer.Internal;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.S3
{
    /// <summary>
    /// A S3Bucket
    /// </summary>
    public partial class S3Bucket
    {
        #region Fields
        private AmazonS3Client _s3;
        private String _bucketName;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a S3 Bucket
        /// </summary>
        public S3Bucket(String accessKey, String secretAccessKey, String bucketName)
        {
            _s3 = new AmazonS3Client(accessKey, secretAccessKey);
            _bucketName = bucketName;
        }
        #endregion
    }
}
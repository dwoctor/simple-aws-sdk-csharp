using Amazon.S3;
using Amazon.S3.Internal;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Transfer.Internal;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.S3
{
    /// <summary>
    /// A S3Bucket
    /// </summary>
    public class S3Bucket
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

        #region Methods
        public Boolean Find(S3ObjectKey key)
        {
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
            listObjectsRequest.WithBucketName(_bucketName);
            listObjectsRequest.WithPrefix(key.Value);
            ListObjectsResponse listObjectsResponse = _s3.ListObjects(listObjectsRequest);
            return listObjectsResponse.S3Objects.Exists(x => x.Key == key.Value);
        }

        public void Put(S3Object putObject)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest();
            putObjectRequest.WithBucketName(_bucketName);
            putObjectRequest.WithInputStream(putObject.DataStream);
            putObjectRequest.StorageClass = putObject.StorageClass;
            putObjectRequest.WithKey(putObject.Key.Value);
            putObjectRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
            putObjectRequest.Timeout = -1;
            S3Response s3Response = _s3.PutObject(putObjectRequest);
        }

        public S3Object Get(S3ObjectKey key)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest();
            getObjectRequest.WithBucketName(_bucketName);
            getObjectRequest.WithKey(key.Value);
            S3Response s3Response = _s3.GetObject(getObjectRequest);
            if (s3Response != null && s3Response.ResponseStream.Length > 0)
            {
                return new S3Object(key.Value, s3Response.ResponseStream);
            }
            else
            {
                return null;
            }
        }

        public void Delete(String key)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.WithBucketName(_bucketName);
            deleteObjectRequest.WithKey(key);
            S3Response s3Response = _s3.DeleteObject(deleteObjectRequest);
        }

        public void Delete(S3Object putObject)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.WithBucketName(_bucketName);
            deleteObjectRequest.WithKey(putObject.Key.Value);
            S3Response s3Response = _s3.DeleteObject(deleteObjectRequest);
        }
        #endregion
    }
}
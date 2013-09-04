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
        #region Methods
        public Boolean Find(S3ObjectKey key)
        {
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
            listObjectsRequest.BucketName = _bucketName;
            listObjectsRequest.Prefix = key.Value;
            ListObjectsResponse listObjectsResponse = _s3.ListObjects(listObjectsRequest);
            return listObjectsResponse.S3Objects.Exists(x => x.Key == key.Value);
        }

        public void Put(S3Object putObject)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest();
            putObjectRequest.BucketName = _bucketName;
            putObjectRequest.InputStream = putObject.DataStream;
            putObjectRequest.StorageClass = putObject.StorageClass;
            putObjectRequest.Key = putObject.Key.Value;
            putObjectRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
            PutObjectResponse putObjectResponse = _s3.PutObject(putObjectRequest);
        }

        public S3Object Get(S3ObjectKey key)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest();
            getObjectRequest.BucketName = _bucketName;
            getObjectRequest.Key =key.Value;
            GetObjectResponse getObjectResponse = _s3.GetObject(getObjectRequest);
            if (getObjectResponse != null && getObjectResponse.ResponseStream.Length > 0)
            {
                return new S3Object(key.Value, getObjectResponse.ResponseStream);
            }
            else
            {
                return null;
            }
        }

        public void Delete(String key)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.BucketName = _bucketName;
            deleteObjectRequest.Key = key;
            DeleteObjectResponse deleteObjectResponse = _s3.DeleteObject(deleteObjectRequest);
        }

        public void Delete(S3Object putObject)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.BucketName = _bucketName;
            deleteObjectRequest.Key = putObject.Key.Value;
            DeleteObjectResponse deleteObjectResponse = _s3.DeleteObject(deleteObjectRequest);
        }
        #endregion
    }
}
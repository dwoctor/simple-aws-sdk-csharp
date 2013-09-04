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
        public async Task<Boolean> FindAsync(S3ObjectKey key)
        {
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
            listObjectsRequest.BucketName = _bucketName;
            listObjectsRequest.Prefix = key.Value;
            ListObjectsResponse listObjectsResponse = await _s3.ListObjectsAsync(listObjectsRequest);
            return listObjectsResponse.S3Objects.Exists(x => x.Key == key.Value);
        }

        public async void PutAsync(S3Object putObject)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest();
            putObjectRequest.BucketName = _bucketName;
            putObjectRequest.InputStream = putObject.DataStream;
            putObjectRequest.StorageClass = putObject.StorageClass;
            putObjectRequest.Key = putObject.Key.Value;
            putObjectRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
            PutObjectResponse putObjectResponse = await _s3.PutObjectAsync(putObjectRequest);
        }

        public async Task<S3Object> GetAsync(S3ObjectKey key)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest();
            getObjectRequest.BucketName = _bucketName;
            getObjectRequest.Key = key.Value;
            GetObjectResponse getObjectResponse = await _s3.GetObjectAsync(getObjectRequest);
            if (getObjectResponse != null && getObjectResponse.ResponseStream.Length > 0)
            {
                return new S3Object(key.Value, getObjectResponse.ResponseStream);
            }
            else
            {
                return null;
            }
        }

        public async void DeleteAsync(String key)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.BucketName = _bucketName;
            deleteObjectRequest.Key = key;
            DeleteObjectResponse deleteObjectResponse = await _s3.DeleteObjectAsync(deleteObjectRequest);
        }

        public async void DeleteAsync(S3Object putObject)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.BucketName = _bucketName;
            deleteObjectRequest.Key = putObject.Key.Value;
            DeleteObjectResponse deleteObjectResponse = await _s3.DeleteObjectAsync(deleteObjectRequest);
        }
        #endregion
    }
}
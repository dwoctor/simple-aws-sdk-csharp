using Amazon.S3;
using Amazon.S3.Model;
using AWS.S3;
using AWS.S3.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                _s3 = new AmazonS3Client(accessKey, secretAccessKey);
                Name = bucketName;
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }
        #endregion

        #region Methods
        
        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a bucket associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="bucketName">The name of the bucket to create</param>
        public static S3Bucket CreateBucket(String accessKey, String secretAccessKey, String bucketName)
        {
            try
            {
                AmazonS3Client s3 = new AmazonS3Client(accessKey, secretAccessKey);
                PutBucketResponse putBucketResponse = s3.PutBucket(new PutBucketRequest() { BucketName = bucketName, UseClientRegion = true });
                return new S3Bucket(accessKey, secretAccessKey, bucketName);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a bucket associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="bucketName">The name of the bucket</param>
        public static void DeleteBucket(String accessKey, String secretAccessKey, String bucketName)
        {
            try
            {
                AmazonS3Client s3 = new AmazonS3Client(accessKey, secretAccessKey);
                DeleteBucketResponse deleteBucketResponse = s3.DeleteBucket(new DeleteBucketRequest() { BucketName = bucketName, UseClientRegion = true });
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        /// <summary>
        /// Gets a list of all the buckets associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        public static List<S3Bucket> GetListOfBuckets(String accesskey, String secretAccessKey)
        {
            try
            {
                List<S3Bucket> buckets = new List<S3Bucket>();
                AmazonS3Client s3 = new AmazonS3Client(accesskey, secretAccessKey);
                ListBucketsResponse listBucketsResponse = s3.ListBuckets(new ListBucketsRequest());
                foreach (var bucket in listBucketsResponse.Buckets)
                {
                    buckets.Add(new S3Bucket(accesskey, secretAccessKey, bucket.BucketName));
                }
                return buckets;
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public Boolean Find(S3ObjectKey key)
        {
            try
            {
                ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
                listObjectsRequest.BucketName = this.Name;
                listObjectsRequest.Prefix = key.Value;
                ListObjectsResponse listObjectsResponse = _s3.ListObjects(listObjectsRequest);
                return listObjectsResponse.S3Objects.Exists(x => x.Key == key.Value);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public void Put(S3Object putObject)
        {
            try
            {
                PutObjectRequest putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = this.Name;
                putObjectRequest.InputStream = putObject.DataStream;
                putObjectRequest.StorageClass = putObject.StorageClass;
                putObjectRequest.Key = putObject.Key.Value;
                putObjectRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
                PutObjectResponse putObjectResponse = _s3.PutObject(putObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public S3Object Get(S3ObjectKey key)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest();
                getObjectRequest.BucketName = this.Name;
                getObjectRequest.Key = key.Value;
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
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public void Delete(String key)
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = this.Name;
                deleteObjectRequest.Key = key;
                DeleteObjectResponse deleteObjectResponse = _s3.DeleteObject(deleteObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public void Delete(S3Object putObject)
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = this.Name;
                deleteObjectRequest.Key = putObject.Key.Value;
                DeleteObjectResponse deleteObjectResponse = _s3.DeleteObject(deleteObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }
#endif
        #endregion

        #region Async
#if NET45
        /// <summary>
        /// Creates a bucket associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="bucketName">The name of the bucket to create</param>
        public async static Task<S3Bucket> CreateBucketAsync(String accessKey, String secretAccessKey, String bucketName)
        {
            try
            {
                AmazonS3Client s3 = new AmazonS3Client(accessKey, secretAccessKey);
                PutBucketResponse putBucketResponse = await s3.PutBucketAsync(new PutBucketRequest() { BucketName = bucketName, UseClientRegion = true });
                return new S3Bucket(accessKey, secretAccessKey, bucketName);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a bucket associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        /// <param name="bucketName">The name of the bucket</param>
        public async static Task DeleteBucketAsync(String accessKey, String secretAccessKey, String bucketName)
        {
            try
            {
                AmazonS3Client s3 = new AmazonS3Client(accessKey, secretAccessKey);
                DeleteBucketResponse deleteBucketResponse = await s3.DeleteBucketAsync(new DeleteBucketRequest() { BucketName = bucketName, UseClientRegion = true });
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        /// <summary>
        /// Gets a list of all the buckets associated with an aws account
        /// </summary>
        /// <param name="accessKey">The access key of the aws account</param>
        /// <param name="secretAccessKey">The secret access key of the aws account</param>
        public async static Task<List<S3Bucket>> GetListOfBucketsAsync(String accesskey, String secretAccessKey)
        {
            try
            {
                List<S3Bucket> buckets = new List<S3Bucket>();
                AmazonS3Client s3 = new AmazonS3Client(accesskey, secretAccessKey);
                ListBucketsResponse listBucketsResponse = await s3.ListBucketsAsync(new ListBucketsRequest());
                foreach (var bucket in listBucketsResponse.Buckets)
                {
                    buckets.Add(new S3Bucket(accesskey, secretAccessKey, bucket.BucketName));
                }
                return buckets;
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public async Task<Boolean> FindAsync(S3ObjectKey key)
        {
            try
            {
                ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
                listObjectsRequest.BucketName = this.Name;
                listObjectsRequest.Prefix = key.Value;
                ListObjectsResponse listObjectsResponse = await _s3.ListObjectsAsync(listObjectsRequest);
                return listObjectsResponse.S3Objects.Exists(x => x.Key == key.Value);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public async void PutAsync(S3Object putObject)
        {
            try
            {
                PutObjectRequest putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = this.Name;
                putObjectRequest.InputStream = putObject.DataStream;
                putObjectRequest.StorageClass = putObject.StorageClass;
                putObjectRequest.Key = putObject.Key.Value;
                putObjectRequest.ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256;
                PutObjectResponse putObjectResponse = await _s3.PutObjectAsync(putObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public async Task<S3Object> GetAsync(S3ObjectKey key)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest();
                getObjectRequest.BucketName = this.Name;
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
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public async void DeleteAsync(String key)
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = this.Name;
                deleteObjectRequest.Key = key;
                DeleteObjectResponse deleteObjectResponse = await _s3.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }

        public async void DeleteAsync(S3Object putObject)
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest();
                deleteObjectRequest.BucketName = this.Name;
                deleteObjectRequest.Key = putObject.Key.Value;
                DeleteObjectResponse deleteObjectResponse = await _s3.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (Exception error)
            {
                throw S3BucketException.Generator(error);
            }
        }
#endif
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// The name of the bucket
        /// </summary>
        public String Name
        {
            get
            {
                try
                {
                    return _bucketName;
                }
                catch (Exception error)
                {
                    throw S3BucketException.Generator(error);
                }
            }
            internal set
            {
                try
                {
                    _bucketName = value;
                }
                catch (Exception error)
                {
                    throw S3BucketException.Generator(error);
                }
            }
        }
        #endregion
    }
}
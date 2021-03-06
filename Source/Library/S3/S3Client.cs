﻿using AWS.S3;
using AWS.S3.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWS.S3
{
    /// <summary>
    /// A S3Client
    /// </summary>
    public class S3Client
    {
        #region Fields
        /// <summary>
        /// Stores the AWS Credentials
        /// </summary>
        private AWSCredentials _awsCredentials;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a S3Client
        /// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        public S3Client(AWSCredentials awsCredentials)
        {
            try
            {
                _awsCredentials = awsCredentials;
                Buckets = new List<S3Bucket>();
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }
        #endregion

        /// <summary>
        /// Get a bucket
        /// </summary>
        /// <param name="bucketName">The name of the bucket to get</param>
        /// <returns>S3Bucket</returns>
        [System.Runtime.CompilerServices.IndexerName("S3ClientIndex")]
        public S3Bucket this[String bucketName]
        {
            get
            {
                try
                {
                    return Buckets.Single(x => x.Name == bucketName);
                }
                catch (Exception error)
                {
                    throw S3ClientException.Generator(error);
                }
            }
        }

        #region Properties
        /// <summary>
        /// The buckets
        /// </summary>
        public List<S3Bucket> Buckets { get; private set; }
        #endregion

        #region Methods

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a bucket
        /// </summary>
        /// <param name="bucketName">The name of the bucket to create</param>
        public S3Bucket CreateTable(String queueName)
        {
            try
            {
                S3Bucket bucket = S3Bucket.CreateBucket(_awsCredentials, queueName);
                Buckets.Add(bucket);
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
                return bucket;
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a bucket
        /// </summary>
        /// <param name="bucketName">The name of the bucket to delete</param>
        public void DeleteBucket(String bucketName)
        {
            try
            {
                S3Bucket.DeleteBucket(_awsCredentials, bucketName);
                Buckets.RemoveAll(x => x.Name == bucketName);
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }

        /// <summary>
        /// Gets all the queues
        /// </summary>
        public void GetQueues()
        {
            try
            {
                List<S3Bucket> buckets = S3Bucket.GetListOfBuckets(_awsCredentials);
                foreach (var bucket in buckets)
                {
                    if (Buckets.Any(x => x.Name == bucket.Name) == false)
                    {
                        Buckets.Add(bucket);
                    }
                }
                foreach (var bucket in Buckets)
                {
                    if (Buckets.Any(x => x.Name == bucket.Name) == false)
                    {
                        Buckets.RemoveAll(x => x.Name == bucket.Name);
                    }
                }
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }
#endif
        #endregion

        #region Async
#if NET45
        /// <summary>
        /// Creates a bucket
        /// </summary>
        /// <param name="bucketName">The name of the bucket to create</param>
        public async Task<S3Bucket> CreateBucketAsync(String bucketName)
        {
            try
            {
                S3Bucket bucket = await S3Bucket.CreateBucketAsync(_awsCredentials, bucketName);
                Buckets.Add(bucket);
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
                return bucket;
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a bucket
        /// </summary>
        /// <param name="bucketName">The name of the bucket to delete</param>
        public async void DeleteBucketAsync(String bucketName)
        {
            try
            {
                await S3Bucket.DeleteBucketAsync(_awsCredentials, bucketName);
                Buckets.RemoveAll(x => x.Name == bucketName);
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }

        /// <summary>
        /// Gets all the buckets
        /// </summary>
        public async void GetBucketsAsync()
        {
            try
            {
                List<S3Bucket> buckets = await S3Bucket.GetListOfBucketsAsync(_awsCredentials);
                foreach (var bucket in buckets)
                {
                    if (Buckets.Any(x => x.Name == bucket.Name) == false)
                    {
                        Buckets.Add(bucket);
                    }
                }
                foreach (var bucket in Buckets)
                {
                    if (Buckets.Any(x => x.Name == bucket.Name) == false)
                    {
                        Buckets.RemoveAll(x => x.Name == bucket.Name);
                    }
                }
                Buckets = Buckets.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw S3ClientException.Generator(error);
            }
        }
#endif
        #endregion

        #endregion
    }
}
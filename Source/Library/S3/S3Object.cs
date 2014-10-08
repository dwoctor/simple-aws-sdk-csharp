using Amazon.S3;
using AWS.S3;
using AWS.S3.Exceptions;
using System;
using System.IO;

namespace AWS.S3
{
    /// <summary>
    /// A S3Object
    /// </summary>
    public class S3Object
    {
        #region Fields
        /// <summary>
        /// The object's data
        /// </summary>
        private Byte[] _data;

        /// <summary>
        /// The object's data stream
        /// </summary>
        private Stream _dataStream;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs an S3Object
        /// </summary>
        public S3Object(String key, Byte[] data, Boolean reducedRedundancyStorage = false)
        {
            try
            {
                this.Key = new S3ObjectKey(key);
                this.Data = data;
                if (reducedRedundancyStorage) this.StorageClass = S3StorageClass.ReducedRedundancy;
                else this.StorageClass = S3StorageClass.Standard;
            }
            catch (Exception error)
            {
                throw S3ObjectException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs an S3Object
        /// </summary>
        public S3Object(String key, Stream dataStream, Boolean reducedRedundancyStorage = false)
        {
            try
            {
                this.Key = new S3ObjectKey(key);
                this.DataStream = dataStream;
                if (reducedRedundancyStorage) this.StorageClass = S3StorageClass.ReducedRedundancy;
                else this.StorageClass = S3StorageClass.Standard;
            }
            catch (Exception error)
            {
                throw S3ObjectException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The S3Object's Key
        /// </summary>
        public S3ObjectKey Key { get; set; }

        /// <summary>
        /// The S3Object's Data
        /// </summary>
        public Byte[] Data
        {
            get
            {
                try
                {
                    return _data;
                }
                catch (Exception error)
                {
                    throw S3ObjectException.Generator(error);
                }
            }
            set
            {
                try
                {
                    _data = value;
                    _dataStream = new MemoryStream(_data);
                }
                catch (Exception error)
                {
                    throw S3ObjectException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The S3Object's Data Stream
        /// </summary>
        internal Stream DataStream
        {
            get
            {
                try
                {
                    return _dataStream;
                }
                catch (Exception error)
                {
                    throw S3ObjectException.Generator(error);
                }
            }
            private set
            {
                try
                {
                    using (value)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
						    value.CopyTo(memoryStream);
                            _data = memoryStream.ToArray();
                            _dataStream = memoryStream;
                        }
                    }
                }
                catch (Exception error)
                {
                    throw S3ObjectException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The S3Object's StorageClass
        /// </summary>
        internal S3StorageClass StorageClass { get; private set; }
        #endregion
    }
}
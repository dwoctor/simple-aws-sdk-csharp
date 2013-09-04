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
            this.Key = new S3ObjectKey(key);
            this.Data = data;
            if (reducedRedundancyStorage) this.StorageClass = S3StorageClass.ReducedRedundancy;
            else this.StorageClass = S3StorageClass.Standard;
        }

        /// <summary>
        /// Constructs an S3Object
        /// </summary>
        public S3Object(String key, Stream dataStream, Boolean reducedRedundancyStorage = false)
        {
            this.Key = new S3ObjectKey(key);
            this.DataStream = dataStream;
            if (reducedRedundancyStorage) this.StorageClass = S3StorageClass.ReducedRedundancy;
            else this.StorageClass = S3StorageClass.Standard;
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
                return _data;
            }
            set
            {
                _data = value;
                _dataStream = new MemoryStream(_data);
            }
        }

        /// <summary>
        /// The S3Object's Data Stream
        /// </summary>
        internal Stream DataStream
        {
            get
            {
                return _dataStream;
            }
            private set
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
        }

        /// <summary>
        /// The S3Object's StorageClass
        /// </summary>
        internal S3StorageClass StorageClass { get; private set; }
        #endregion
    }
}
using System;
using Amazon;

namespace AWS
{
    /// <summary>
    /// AWSCredentials
    /// </summary>
    public class AWSCredentials
    {
        /// <summary>
        /// The AWS regions
		/// </summary>
		public enum Regions
		{
            /// <summary>
            /// The APNortheast1 Region
            /// </summary>
			APNortheast1,
            /// <summary>
            /// The APSoutheast1 Region
            /// </summary>
			APSoutheast1,
            /// <summary>
            /// The APSoutheast2 Region
            /// </summary>
			APSoutheast2,
            /// <summary>
            /// The EUWest1 Region
            /// </summary>
			EUWest1,
            /// <summary>
            /// The SAEast1 Region
            /// </summary>
			SAEast1,
            /// <summary>
            /// The USEast1 Region
            /// </summary>
			USEast1,
            /// <summary>
            /// The USGovCloudWest1 Region
            /// </summary>
			USGovCloudWest1,
            /// <summary>
            /// The USWest1 Region
            /// </summary>
			USWest1,
            /// <summary>
            /// The USWest2 Region
            /// </summary>
			USWest2,
		};	 

        #region Constructors
        /// <summary>
        /// Constructs a AWSCredentials
        /// </summary>
        /// <param name="accessKey">An aws access key</param>
        /// <param name="secretAccessKey">An aws secret access key</param>
        /// <param name="region">An aws region</param>
        public AWSCredentials(String accessKey, String secretAccessKey, Regions region)
        {
            this.AccessKey = accessKey;
            this.SecretAccessKey = secretAccessKey;
            switch(region)
            {
                case Regions.APNortheast1:
                    this.Region = RegionEndpoint.APNortheast1;
                    break;
                case Regions.APSoutheast1:
                    this.Region = RegionEndpoint.APSoutheast1;
                    break;
                case Regions.APSoutheast2:
                    this.Region = RegionEndpoint.APSoutheast2;
                    break;
                case Regions.EUWest1:
                    this.Region = RegionEndpoint.EUWest1;
                    break;
                case Regions.SAEast1:
                    this.Region = RegionEndpoint.SAEast1;
                    break;
                case Regions.USEast1:
                    this.Region = RegionEndpoint.USEast1;
                    break;
                case Regions.USGovCloudWest1:
                    this.Region = RegionEndpoint.USGovCloudWest1;
                    break;
                case Regions.USWest1:
                    this.Region = RegionEndpoint.USWest1;
                    break;
                case Regions.USWest2:
                    this.Region = RegionEndpoint.USWest2;
                    break;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Stores the aws access key
        /// </summary>
        internal String AccessKey { get; private set; }

        /// <summary>
        /// Stores the aws secret access key
        /// </summary>
        internal String SecretAccessKey { get; private set; }

        /// <summary>
        /// Stores the aws region
        /// </summary>
        internal RegionEndpoint Region { get; private set; }
        #endregion
    }
}
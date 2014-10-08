using System;

namespace AWS.Tests
{
    public class TestData
    {

        #region Fields
        /// <summary>
        /// Stores the AWSCredentials
        /// </summary>
        private AWSCredentials _credentials;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a DynamoDBTestData object
        /// </summary>
        public TestData()
        {
            this.Credentials = new AWSCredentials(<Insert AWS Access Key Here>, <Insert AWS Secret Access Key Here>, <Select your AWS Region Here>);
            this.WaitTime = 60000;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get and Set the AWSCredentials
        /// </summary>
        public AWSCredentials Credentials
        {
            get
            {
                return this._credentials;
            }
            protected set
            {
                this._credentials = value;
            }
        }

        /// <summary>
        /// The WaitTime
        /// </summary>
        public Int32 WaitTime { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the AWSCredentials
        /// </summary>
        /// <param name="accessKey">An aws access key</param>
        /// <param name="secretAccessKey">An aws secret access key</param>
        /// <param name="region">An aws region</param>
        public void UpdateCredentials(String accessKey, String secretAccessKey, AWSCredentials.Regions region)
        {
            this.Credentials = new AWSCredentials(accessKey, secretAccessKey, region);
        }
        #endregion

    }
}

using AWS;
using System;

namespace AWS.Tests.Library
{
    public class TestCredentials
    {
        public static AWSCredentials Credentials = new AWSCredentials("<Insert AWS Access Key Here>",
                                                                       "<Insert AWS Secret Access Key Here>",
                                       /*Select your AWS Region Here*/ AWSCredentials.Regions.USEast1);
    }
}
﻿using AWS.SimpleDB;
using AWS.SimpleDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AWS.SimpleDB
{
    /// <summary>
    /// A SimpleDBClient
    /// </summary>
    public class SimpleDBClient
    {
        #region Fields
        /// <summary>
        /// Stores the AWS Credentials
        /// </summary>
        private AWSCredentials _awsCredentials;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a SimpleDBClient
		/// </summary>
        /// <param name="awsCredentials">AWS Credentials</param>
        public SimpleDBClient(AWSCredentials awsCredentials)
		{
            try
            {
                _awsCredentials = awsCredentials;
                Domains = new List<SimpleDBDomain>();
#if !WINDOWSPHONE && !WINRT
                GetDomains();
#endif
            }
            catch (Exception error)
            {
                throw SimpleDBClientException.Generator(error);
            }
		}
		#endregion

        #region Methods

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a domain associated with an AWS account
        /// </summary>
        /// <param name="domainName">The name of the domain to create</param>
        public SimpleDBDomain CreateDomain(String domainName)
        {
            try
            {
                SimpleDBDomain domain = SimpleDBDomain.CreateDomain(_awsCredentials, domainName);
                Domains.Add(domain);
                Domains = Domains.OrderBy(x => x.Name).ToList();
                return domain;
            }
            catch (Exception error)
            {
                throw SimpleDBClientException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a domain associated with an AWS account
        /// </summary>
        /// <param name="domainName">The name of the domain to delete</param>
        public void DeleteTable(String domainName)
        {
            try
            {
                SimpleDBDomain.DeleteDomain(_awsCredentials, domainName);
                Domains.RemoveAll(x => x.Name == domainName);
                Domains = Domains.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw SimpleDBClientException.Generator(error);
            }
        }

        /// <summary>
        /// Gets all the Domains
        /// </summary>
        public void GetDomains()
        {
            try
            {
                List<SimpleDBDomain> domains = SimpleDBDomain.ListDomains(_awsCredentials);
                foreach (var domain in domains)
                {
                    if (Domains.Any(x => x.Name == domain.Name) == false)
                    {
                        Domains.Add(domain);
                    }
                }
                foreach (var domain in Domains)
                {
                    if (domains.Any(x => x.Name == domain.Name) == false)
                    {
                        Domains.RemoveAll(x => x.Name == domain.Name);
                    }
                }
                Domains = Domains.OrderBy(x => x.Name).ToList();
            }
            catch (Exception error)
            {
                throw SimpleDBClientException.Generator(error);
            }
        }
#endif
        #endregion

        /// <summary>
        /// Get a domain
        /// </summary>
        /// <param name="name">The name of the domain to get</param>
        /// <returns>SimpleDBDomain</returns>
        [System.Runtime.CompilerServices.IndexerName("SimpleDBClientIndex")]
        public SimpleDBDomain this[String name]
        {
            get
            {
                try
                {
                    return Domains.Single(x => x.Name == name);
                }
                catch (Exception error)
                {
                    throw SimpleDBClientException.Generator(error);
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The domains
        /// </summary>
        public List<SimpleDBDomain> Domains { get; private set; }
        #endregion
    }
}
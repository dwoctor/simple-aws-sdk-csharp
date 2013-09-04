using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.SimpleDB.Model.Internal;
using Amazon.SimpleDB.Model.Internal.MarshallTransformations;
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
        /// Stores the AWS access key
        /// </summary>
		private String _accessKey;

        /// <summary>
        /// Stores the AWS secret access key
        /// </summary>
		private String _secretAccessKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a SimpleDBClient
		/// </summary>
		public SimpleDBClient(String accessKey, String secretAccessKey)
		{
			_accessKey = accessKey;
			_secretAccessKey = secretAccessKey;
            Domains = new List<SimpleDBDomain>();
            GetDomains();
		}
		#endregion

        #region Methods
        /// <summary>
        /// Creates a domain associated with an AWS account
        /// </summary>
        /// <param name="domainName">The name of the domain to create</param>
        public SimpleDBDomain CreateDomain(String domainName)
        {
            SimpleDBDomain domain = SimpleDBDomain.CreateDomain(_accessKey, _secretAccessKey, domainName);
            Domains.Add(domain);
            Domains = Domains.OrderBy(x => x.Name).ToList();
            return domain;
        }

        /// <summary>
        /// Delete a domain associated with an AWS account
        /// </summary>
        /// <param name="domainName">The name of the domain to delete</param>
        public void DeleteTable(String domainName)
        {
            SimpleDBDomain.DeleteDomain(_accessKey, _secretAccessKey, domainName);
            Domains.RemoveAll(x => x.Name == domainName);
            Domains = Domains.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets all the Domains
        /// </summary>
        public void GetDomains()
        {
            List<SimpleDBDomain> domains = SimpleDBDomain.ListDomains(_accessKey, _secretAccessKey);
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
                return Domains.Single(x => x.Name == name);
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
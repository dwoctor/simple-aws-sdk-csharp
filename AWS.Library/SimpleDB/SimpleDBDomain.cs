using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.SimpleDB.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.SimpleDB
{
    /// <summary>
    /// A SimpleDBDomain
    /// </summary>
    public class SimpleDBDomain
    {
        #region Fields
        /// <summary>
        /// Amazon SimpleDB client
        /// </summary>
        AmazonSimpleDBClient _sdb;

        /// <summary>
        /// The name of the domain.
        /// </summary>
        private String _domainName;
        #endregion

        #region Constructors
        /// <summary>
        /// Upon Construction of the Queue
        /// </summary>
        public SimpleDBDomain(String accessKey, String secretAccessKey, String domainName)
        {
            _sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
            _domainName = domainName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a domain associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="domainName">The name of the domain to create</param>
        public static SimpleDBDomain CreateDomain(String accessKey, String secretAccessKey, String domainName)
        {
            AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
            CreateDomainRequest createDomainRequest = new CreateDomainRequest();
            createDomainRequest.DomainName = domainName;
            CreateDomainResponse createDomainResponse = sdb.CreateDomain(createDomainRequest);
            return new SimpleDBDomain(accessKey, secretAccessKey, domainName);
        }

        /// <summary>
        /// Delete a domain associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="domainName">The name of the domain to delete</param>
        public static void DeleteDomain(String accessKey, String secretAccessKey, String domainName)
        {
            AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
            DeleteDomainRequest deleteDomainRequest = new DeleteDomainRequest();
            deleteDomainRequest.DomainName = domainName;
            DeleteDomainResponse deleteDomainResponse = sdb.DeleteDomain(deleteDomainRequest);
        }

        /// <summary>
        /// Gets a list of domains associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static List<SimpleDBDomain> ListDomains(String accessKey, String secretAccessKey)
        {
            AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
            List<SimpleDBDomain> domains = new List<SimpleDBDomain>();
            ListDomainsRequest listDomainsRequest = new ListDomainsRequest();
            ListDomainsResponse listDomainsResponse = sdb.ListDomains(listDomainsRequest);
            if (listDomainsResponse.IsSetListDomainsResult())
            {
                ListDomainsResult listDomainsResult = listDomainsResponse.ListDomainsResult;
                foreach (var domainName in listDomainsResult.DomainName)
                {
                    domains.Add(new SimpleDBDomain(accessKey, secretAccessKey, domainName));
                }
            }
            return domains;
        }

        /// <summary>
        /// Puts an item in the domain
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public void PutItem(SimpleDBItem item)
        {
            PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
            putAttributesRequest.DomainName = _domainName;
            putAttributesRequest.ItemName = item.Name;
            putAttributesRequest.Attribute = item.ToListOfReplaceableAttributes();
            PutAttributesResponse putDomainResponse = _sdb.PutAttributes(putAttributesRequest);
        }

        #region GetItem
        /// <summary>
        /// Gets an item from the domain
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName)
        {
            return GetItem(itemName, new List<String>());
        }

        /// <summary>
        /// Gets an item from the domain
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding an item in the domain</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName, String returnAttribute)
        {
            return GetItem(itemName, new List<String>() { returnAttribute });
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// /// <param name="returnAttributes">An attribute of the item to to be returned upon finding an item in the domain</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName, List<String> returnAttributes)
        {
            SimpleDBItem returnItem = new SimpleDBItem(itemName);
            GetAttributesRequest getAttributesRequest = new GetAttributesRequest();
            getAttributesRequest.DomainName = _domainName;
            getAttributesRequest.ItemName = itemName;
            getAttributesRequest.AttributeName = returnAttributes;
            GetAttributesResponse getAttributesResponse = _sdb.GetAttributes(getAttributesRequest);
            foreach (Amazon.SimpleDB.Model.Attribute attribute in getAttributesResponse.GetAttributesResult.Attribute)
            {
                returnItem.Add(new SimpleDBAttribute(attribute));
            }
            return returnItem;
        }
        #endregion

        //#region SelectItemNames
        //public List<String> SelectItemNames(String selectExpression)
        //{
        //    List<String> itemNames = new List<String>();
        //    foreach (var item in SelectItem(selectExpression))
        //    {
        //        itemNames.Add(item.Name);
        //    }
        //    return itemNames;
        //}

        //public List<SimpleDBItem> SelectItem(String selectExpression)
        //{
        //    SelectRequest selectRequest = new SelectRequest();
        //    selectRequest.SelectExpression = selectExpression;
        //    SelectResponse selectResponse = _sdb.Select(selectRequest);
        //    SelectResult selectResult = selectResponse.SelectResult;
        //    return selectResult.Item;
        //}
        //#endregion

        #region DeleteItem
        /// <summary>
        /// Deletes an item from the domain
        /// </summary>
        /// <param name="itemName">The name of the item</param>
        public void DeleteItem(String itemName)
        {
            DeleteAttributesRequest deleteAttributesRequest = new DeleteAttributesRequest();
            deleteAttributesRequest.DomainName = _domainName;
            deleteAttributesRequest.ItemName = itemName;
            DeleteAttributesResponse deleteDomainResponse = _sdb.DeleteAttributes(deleteAttributesRequest);
        }

        ///// <summary>
        ///// Deletes an item from the domain
        ///// </summary>
        ///// <param name="itemName">The name of the item</param>
        ///// <param name="attributes"></param>
        //public void DeleteItem(String itemName, List<SimpleDBAttribute> attributes)
        //{
        //    DeleteAttributesRequest deleteAttributesRequest = new DeleteAttributesRequest();
        //    deleteAttributesRequest.DomainName = _domainName;
        //    deleteAttributesRequest.ItemName = itemName;
        //    deleteAttributesRequest.Attribute = attributes;
        //    DeleteAttributesResponse deleteDomainResponse = _sdb.DeleteAttributes(deleteAttributesRequest);
        //}
        #endregion

        /// <summary>
        /// Returns the name of the SimpleDBDomain
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            return _domainName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the SimpleDBDomain
        /// </summary>
        public String Name { get { return _domainName; } }
        #endregion
    }
}
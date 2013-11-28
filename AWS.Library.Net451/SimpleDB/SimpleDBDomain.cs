using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using AWS.SimpleDB;
using AWS.SimpleDB.Exceptions;
using System;
using System.Collections.Generic;

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

        #region Normal
#if !WINDOWSPHONE && !WINRT
        /// <summary>
        /// Creates a domain associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="domainName">The name of the domain to create</param>
        public static SimpleDBDomain CreateDomain(String accessKey, String secretAccessKey, String domainName)
        {
            try
            {
                AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
                CreateDomainRequest createDomainRequest = new CreateDomainRequest();
                createDomainRequest.DomainName = domainName;
                CreateDomainResponse createDomainResponse = sdb.CreateDomain(createDomainRequest);
                return new SimpleDBDomain(accessKey, secretAccessKey, domainName);
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Delete a domain associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        /// <param name="domainName">The name of the domain to delete</param>
        public static void DeleteDomain(String accessKey, String secretAccessKey, String domainName)
        {
            try
            {
                AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
                DeleteDomainRequest deleteDomainRequest = new DeleteDomainRequest();
                deleteDomainRequest.DomainName = domainName;
                DeleteDomainResponse deleteDomainResponse = sdb.DeleteDomain(deleteDomainRequest);
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Gets a list of domains associated with an AWS account
        /// </summary>
        /// <param name="accessKey">The Access Key of the AWS account</param>
        /// <param name="secretAccessKey">The Secret Access Key of the AWS account</param>
        public static List<SimpleDBDomain> ListDomains(String accessKey, String secretAccessKey)
        {
            try
            {
                AmazonSimpleDBClient sdb = new AmazonSimpleDBClient(accessKey, secretAccessKey);
                List<SimpleDBDomain> domains = new List<SimpleDBDomain>();
                ListDomainsRequest listDomainsRequest = new ListDomainsRequest();
                ListDomainsResponse listDomainsResponse = sdb.ListDomains(listDomainsRequest);
                foreach (var domainName in listDomainsResponse.DomainNames)
                {
                    domains.Add(new SimpleDBDomain(accessKey, secretAccessKey, domainName));
                }
                return domains;
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Puts an item in the domain
        /// </summary>
        /// <param name="item">The item to put in the table</param>
        public void PutItem(SimpleDBItem item)
        {
            try
            {
                PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
                putAttributesRequest.DomainName = _domainName;
                putAttributesRequest.ItemName = item.Name;
                putAttributesRequest.Attributes = item.ToListOfReplaceableAttributes();
                PutAttributesResponse putDomainResponse = _sdb.PutAttributes(putAttributesRequest);
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        #region GetItem
        /// <summary>
        /// Gets an item from the domain
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName)
        {
            try
            {
                return GetItem(itemName, new List<String>());
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Gets an item from the domain
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// /// <param name="returnAttribute">An attribute of the item to to be returned upon finding an item in the domain</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName, String returnAttribute)
        {
            try
            {
                return GetItem(itemName, new List<String>() { returnAttribute });
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Gets an item from the table
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// /// <param name="returnAttributes">An attribute of the item to to be returned upon finding an item in the domain</param>
        /// <returns>SimpleDBItem</returns>
        public SimpleDBItem GetItem(String itemName, List<String> returnAttributes)
        {
            try
            {
                SimpleDBItem returnItem = new SimpleDBItem(itemName);
                GetAttributesRequest getAttributesRequest = new GetAttributesRequest();
                getAttributesRequest.DomainName = _domainName;
                getAttributesRequest.ItemName = itemName;
                getAttributesRequest.AttributeNames = returnAttributes;
                GetAttributesResponse getAttributesResponse = _sdb.GetAttributes(getAttributesRequest);
                foreach (Amazon.SimpleDB.Model.Attribute attribute in getAttributesResponse.Attributes)
                {
                    returnItem.Add(new SimpleDBAttribute(attribute));
                }
                return returnItem;
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }
        #endregion

        #region SelectItemNames
        public List<String> SelectItemNames(String selectExpression)
        {
            try
            {
                List<String> itemNames = new List<String>();
                foreach (var item in SelectItem(selectExpression))
                {
                    itemNames.Add(item.Name);
                }
                return itemNames;
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        public List<SimpleDBItem> SelectItem(String selectExpression)
        {
            try
            {
                SelectRequest selectRequest = new SelectRequest();
                selectRequest.SelectExpression = selectExpression;
                SelectResponse selectResponse = _sdb.Select(selectRequest);
                List<SimpleDBItem> items = new List<SimpleDBItem>();
                foreach (var item in selectResponse.Items)
                {
                    items.Add(new SimpleDBItem(item.Name, item.Attributes));
                }
                return items;
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }
        #endregion

        #region DeleteItem
        /// <summary>
        /// Deletes an item from the domain
        /// </summary>
        /// <param name="itemName">The name of the item</param>
        public void DeleteItem(String itemName)
        {
            try
            {
                DeleteItem(itemName, new List<SimpleDBAttribute>());
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }

        /// <summary>
        /// Deletes an item from the domain
        /// </summary>
        /// <param name="itemName">The name of the item</param>
        /// <param name="attributes"></param>
        public void DeleteItem(String itemName, List<SimpleDBAttribute> attributes)
        {
            try
            {
                DeleteAttributesRequest deleteAttributesRequest = new DeleteAttributesRequest();
                deleteAttributesRequest.DomainName = _domainName;
                deleteAttributesRequest.ItemName = itemName;
                foreach (var attribute in attributes)
                {
                    deleteAttributesRequest.Attributes.Add(new Amazon.SimpleDB.Model.Attribute() { Name = attribute.Name, Value = attribute.Value });
                }
                DeleteAttributesResponse deleteDomainResponse = _sdb.DeleteAttributes(deleteAttributesRequest);
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }
        #endregion

#endif
        #endregion

        /// <summary>
        /// Returns the name of the SimpleDBDomain
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            try
            {
                return _domainName;
            }
            catch (Exception error)
            {
                throw SimpleDBDomainException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the SimpleDBDomain
        /// </summary>
        public String Name
        {
            get
            {
                try
                {
                    return _domainName;
                }
                catch (Exception error)
                {
                    throw SimpleDBDomainException.Generator(error);
                }
            }
        }
        #endregion
    }
}
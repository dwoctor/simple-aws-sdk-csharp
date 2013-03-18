using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.SimpleDB.Util;
using System;
using System.Collections.Generic;

namespace AWS
{
    public class SimpleDB
    {
        AmazonSimpleDBClient _sdb;

        /// <summary>
        /// Upon Construction of the Queue
        /// </summary>
        public SimpleDB(String accesskey, String secretAccessKey)
        {
            _sdb = new AmazonSimpleDBClient(accesskey, secretAccessKey);
        }

        public List<String> ListDomains()
        {
            ListDomainsRequest listDomainsRequest = new ListDomainsRequest();
            ListDomainsResponse listDomainsResponse = _sdb.ListDomains(listDomainsRequest);
            if (listDomainsResponse.IsSetListDomainsResult())
            {
                ListDomainsResult listDomainsResult = listDomainsResponse.ListDomainsResult;
                return listDomainsResult.DomainName;
            }
            return null;
        }

        public void CreateDomain(String domainName)
        {
            CreateDomainRequest createDomainRequest = new CreateDomainRequest();
            createDomainRequest.DomainName = domainName;
            CreateDomainResponse createDomainResponse = _sdb.CreateDomain(createDomainRequest);
        }

        public void DeleteDomain(String domainName)
        {
            DeleteDomainRequest deleteDomainRequest = new DeleteDomainRequest();
            deleteDomainRequest.DomainName = domainName;
            DeleteDomainResponse deleteDomainResponse = _sdb.DeleteDomain(deleteDomainRequest);
        }

        public void PutItem(String domainName, String attributeName, String attributeValue)
        {
            PutItem(domainName, Guid.NewGuid().ToString(), attributeName, attributeValue);
        }
        public void PutItem(String domainName, String attributeName, String attributeValue, Boolean attributeReplace)
        {
            PutItem(domainName, Guid.NewGuid().ToString(), attributeName, attributeValue, attributeReplace);
        }
        public void PutItem(String domainName, Dictionary<String, String> attributes)
        {
            PutItem(domainName, Guid.NewGuid().ToString(), attributes);
        }
        public void PutItem(String domainName, String itemName, String attributeName, String attributeValue)
        {
            PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
            putAttributesRequest.DomainName = domainName;
            putAttributesRequest.ItemName = itemName;
            putAttributesRequest.Attribute.Add(new ReplaceableAttribute{ Name = attributeName, Value = attributeValue });
            PutAttributesResponse putDomainResponse = _sdb.PutAttributes(putAttributesRequest);
        }
        public void PutItem(String domainName, String itemName, String attributeName, String attributeValue, Boolean attributeReplace)
        {
            PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
            putAttributesRequest.DomainName = domainName;
            putAttributesRequest.ItemName = itemName;
            putAttributesRequest.Attribute.Add(new ReplaceableAttribute { Name = attributeName, Value = attributeValue, Replace = attributeReplace });
            PutAttributesResponse putDomainResponse = _sdb.PutAttributes(putAttributesRequest);
        }
        public void PutItem(String domainName, String itemName, Dictionary<String, String> attributes)
        {
            PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
            putAttributesRequest.DomainName = domainName;
            putAttributesRequest.ItemName = itemName;
            foreach (var item in attributes)
            {
                putAttributesRequest.Attribute.Add(new ReplaceableAttribute { Name = item.Key, Value = item.Value });
            }
            PutAttributesResponse putDomainResponse = _sdb.PutAttributes(putAttributesRequest);
        }

        //public void PutItem(String domainName, String itemName, List<ReplaceableAttribute> attributes)
        //{
        //    PutAttributesRequest putAttributesRequest = new PutAttributesRequest();
        //    putAttributesRequest.DomainName = domainName;
        //    putAttributesRequest.ItemName = itemName;
        //    foreach (var attribute in attributes)
        //    {
        //        putAttributesRequest.Attribute.Add(attribute);
        //    }
        //    PutAttributesResponse putAttributesResponse = _sdb.PutAttributes(putAttributesRequest);
        //}

        //public List<Amazon.SimpleDB.Model.Attribute> GetItem(String domainName, String itemName)
        //{
        //    GetAttributesRequest getAttributesRequest = new GetAttributesRequest();
        //    getAttributesRequest.DomainName = domainName;
        //    getAttributesRequest.ItemName = itemName;
        //    GetAttributesResponse getAttributesResponse = _sdb.GetAttributes(getAttributesRequest);
        //    return getAttributesResponse.GetAttributesResult.Attribute;
        //}

        public Dictionary<String, String> GetItem(String domainName, String itemName)
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            foreach (var attribute in GetAttributes(domainName, itemName))
            {
                attributes.Add(attribute.Name, attribute.Value);
            }
            return attributes;
        }
        public Dictionary<String, String> GetItem(String domainName, String itemName, List<String> attributeNames)
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            foreach (var attribute in GetAttributes(domainName, itemName, attributeNames))
            {
                attributes.Add(attribute.Name, attribute.Value);
            }
            return attributes;
        }

        private List<Amazon.SimpleDB.Model.Attribute> GetAttributes(String domainName, String itemName)
        {
            GetAttributesRequest getAttributesRequest = new GetAttributesRequest();
            getAttributesRequest.DomainName = domainName;
            getAttributesRequest.ItemName = itemName;
            GetAttributesResponse getAttributesResponse = _sdb.GetAttributes(getAttributesRequest);
            return getAttributesResponse.GetAttributesResult.Attribute;
        }
        private List<Amazon.SimpleDB.Model.Attribute> GetAttributes(String domainName, String itemName, List<String> attributeNames)
        {
            GetAttributesRequest getAttributesRequest = new GetAttributesRequest();
            getAttributesRequest.DomainName = domainName;
            getAttributesRequest.ItemName = itemName;
            getAttributesRequest.AttributeName = attributeNames;
            GetAttributesResponse getAttributesResponse = _sdb.GetAttributes(getAttributesRequest);
            return getAttributesResponse.GetAttributesResult.Attribute;
        }

        public List<String> SelectItemNames(String domainName, String selectExpression)
        {
            List<String> itemNames = new List<String>();
            foreach (var item in SelectItem(domainName, selectExpression))
            {
                itemNames.Add(item.Name);
            }
            return itemNames;
        }

        public List<Item> SelectItem(String domainName, String selectExpression)
        {
            SelectRequest selectRequest = new SelectRequest();
            selectRequest.SelectExpression = selectExpression;
            SelectResponse selectResponse = _sdb.Select(selectRequest);
            SelectResult selectResult = selectResponse.SelectResult;
            return selectResult.Item;
        }

        public void DeleteItem(String domainName, String itemName)
        {
            DeleteAttributesRequest deleteAttributesRequest = new DeleteAttributesRequest();
            deleteAttributesRequest.DomainName = domainName;
            deleteAttributesRequest.ItemName = itemName;
            DeleteAttributesResponse deleteDomainResponse = _sdb.DeleteAttributes(deleteAttributesRequest);
        }

        public void DeleteItem(String domainName, String itemName, List<Amazon.SimpleDB.Model.Attribute> attributes)
        {
            DeleteAttributesRequest deleteAttributesRequest = new DeleteAttributesRequest();
            deleteAttributesRequest.DomainName = domainName;
            deleteAttributesRequest.ItemName = itemName;
            deleteAttributesRequest.Attribute = attributes;
            DeleteAttributesResponse deleteDomainResponse = _sdb.DeleteAttributes(deleteAttributesRequest);
        }
    }
}
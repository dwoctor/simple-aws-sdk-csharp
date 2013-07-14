using Amazon;
using Amazon.DynamoDB;
using Amazon.DynamoDB.DataModel;
using Amazon.DynamoDB.DocumentModel;
using Amazon.DynamoDB.Model;
using Amazon.DynamoDB.Model.Internal;
using Amazon.DynamoDB.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBItem
    /// </summary>
    public class DynamoDBItem
    {
        #region Fields
        /// <summary>
        /// Stores the item's attributes
        /// </summary>
        private List<DynamoDBAttribute> _attributes = new List<DynamoDBAttribute>();
        #endregion

        #region Constructors
        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        public DynamoDBItem() { }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="name">The attribute's name to add to the item</param>
        /// <param name="value">The attribute's value to add to the item</param>
        /// <param name="isHashKey">Indicates if the attribute is a Hash Key</param>
        /// /// <param name="isRangeKey">Indicates if the attribute is a Range Key</param>
        public DynamoDBItem(String name, Object value, Boolean isHashKey = false, Boolean isRangeKey = false)
        {
            Add(name, value, isHashKey, isRangeKey);
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attribute">Attribute to add to the item</param>
        public DynamoDBItem(DynamoDBAttribute attribute)
        {
            Add(attribute);
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        public DynamoDBItem(List<DynamoDBAttribute> attributes)
        {
            Add(attributes);
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        public DynamoDBItem(Dictionary<String, Object> attributes)
        {
            Add(attributes);
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="items">Attributes to add to the item</param>
        internal DynamoDBItem(List<Dictionary<String, AttributeValue>> items)
        {
            if (items.Count == 1)
            {
                Add(items[0]);
            }
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        internal DynamoDBItem(Dictionary<String, AttributeValue> attributes)
        {
            Add(attributes);
        }
        #endregion

        #region Methods

        #region Add
        /// <summary>
        /// Adds an attribute to the item
        /// </summary>
        /// <param name="name">The attribute's name</param>
        /// <param name="value">The attribute's value</param>
        /// <param name="isHashKey">Indicates if the attribute is a Hash Key</param>
        /// /// <param name="isRangeKey">Indicates if the attribute is a Range Key</param>
        public void Add(String name, Object value, Boolean isHashKey = false, Boolean isRangeKey = false)
        {
            _attributes.Add(new DynamoDBAttribute(name, value, isHashKey, isRangeKey));
        }

        /// <summary>
        /// Adds an attribute to the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Add(DynamoDBAttribute attribute)
        {
            _attributes.Add(attribute);
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(List<DynamoDBAttribute> attributes)
        {
            _attributes.AddRange(attributes);
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(Dictionary<String, Object> attributes)
        {
            foreach (var attribute in attributes)
            {
                _attributes.Add(new DynamoDBAttribute(attribute.Key, attribute.Value));
            }
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        internal void Add(Dictionary<String, AttributeValue> attributes)
        {
            foreach (var attribute in attributes)
	        {
                _attributes.Add(new DynamoDBAttribute(attribute.Key, attribute.Value));
	        }
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="name">The attribute's name</param>
        /// <param name="value">The attribute's value</param>
        /// <param name="isHashKey">Indicates if the attribute is a Hash Key</param>
        /// <param name="isRangeKey">Indicates if the attribute is a Range Key</param>
        public void Remove(String name, Object value, Boolean isHashKey = false, Boolean isRangeKey = false)
        {
            _attributes.RemoveAll(x => x.Name == name && x.Value == value && x.IsHashKey == isHashKey && x.IsRangeKey == isRangeKey);
        }

        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Remove(DynamoDBAttribute attribute)
        {
            _attributes.Remove(attribute);
        }
        #endregion

        /// <summary>
        /// Does an Attribute Exist 
        /// </summary>
        /// <param name="name">The name of the attribute to find</param>
        /// <returns>Boolean</returns>
        public Boolean ContainsAttribute(String name)
        {
            return _attributes.Any(x => x.Name == name);
        }

        internal Dictionary<String, List<DynamoDBAttribute>> GetAttributesGroupedByName()
        {
            Dictionary<String, List<DynamoDBAttribute>> groupedAttributes = new Dictionary<String, List<DynamoDBAttribute>>();
            foreach (var attribute in _attributes)
	        {
		        if (groupedAttributes.ContainsKey(attribute.Name))
	            {
		            groupedAttributes[attribute.Name].Add(attribute);
	            }
                else
                {
                    groupedAttributes.Add(attribute.Name, new List<DynamoDBAttribute>() { attribute }); 
                }
	        }
            return groupedAttributes;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the AWS.DynamoDB.DynamoDBItem
        /// </summary>
        /// <returns>A AWS.DynamoDB.DynamoDBAttribute.Enumerator for the AWS.DynamoDB.DynamoDBItem</returns>
        public IEnumerator<DynamoDBAttribute> GetEnumerator()
        {
            return this._attributes.GetEnumerator();
        }

        /// <summary>
        /// Get and set the item's attributes
        /// </summary>
        /// <param name="name">The name of the attribute to get or set</param>
        /// <returns>DynamoDBAttribute</returns>
        [System.Runtime.CompilerServices.IndexerName("DynamoDBItemIndex")]
        public DynamoDBAttribute this[String name]
        {
            get
            {
                return _attributes.Single(x => x.Name == name);
            }
            set
            {
                if (name != null && _attributes.Count(x => x.Name == name) == 0)
                {
                    Add(name, value);
                }
                else
                {
                    throw new Exception("Invalid Name");
                }
            }
        }

        /// <summary>
        /// Compare this object with another object
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is DynamoDBItem)
            {
                DynamoDBItem dynamoDBItemObj = obj as DynamoDBItem;
                return (this._attributes == dynamoDBItemObj._attributes);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the Hash Code of the DynamoDBItem
        /// </summary>
        /// <returns>Int32</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Converts the attribute to an AttributeValue object
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal Dictionary<String, AttributeValue> ToDictionary()
        {
            Dictionary<String, AttributeValue> dictionary = new Dictionary<String, AttributeValue>();
            foreach (var attribute in _attributes)
            {
                dictionary.Add(attribute.Name, attribute.ToAttributeValue());
            }
            return dictionary;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The items's names
        /// </summary>
        public List<String> AttributeNames
        {
            get
            {
                return _attributes.Select(x => x.Name).ToList();
            }
        }

        /// <summary>
        /// The attribute's value
        /// </summary>
        public List<Object> AttributeValues
        {
            get
            {
                return _attributes.Select(x => x.Value).ToList();
            }
        }

        /// <summary>
        /// The items's types
        /// </summary>
        public List<Types.Enum> AttributeTypes
        {
            get
            {
                return _attributes.Select(x => x.Type).ToList();
            }
        }

        /// <summary>
        /// The items's Hash Key Value
        /// </summary>
        public DynamoDBAttribute HashKey
        {
            get
            {
                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Single() : null;
            }
            set
            {
                if (HasHashKey) _attributes[_attributes.FindIndex(x => x.IsHashKey == true)] = value;
                else _attributes.Add(value);
            }
        }

        /// <summary>
        /// The items's Hash Key Name
        /// </summary>
        public String HashKeyName
        {
            get
            {
                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.Name).Single() : null;
            }
        }

        /// <summary>
        /// The items's Hash Key Value
        /// </summary>
        public Object HashKeyValue
        {
            get
            {
                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.Value).Single() : null;
            }
        }

        /// <summary>
        /// The items's Hash Key AttributeValue
        /// </summary>
        internal AttributeValue HashKeyAttributeValue
        {
            get
            {
                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.ToAttributeValue()).Single() : null;
            }
        }

        /// <summary>
        /// Does the item have a Hash Key
        /// </summary>
        internal Boolean HasHashKey
        {
            get
            {
                return _attributes.Count(x => x.IsHashKey == true) == 1;
            }
        }

        /// <summary>
        /// The items's Range Key Name
        /// </summary>
        public String RangeKeyName
        {
            get
            {
                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.Name).Single() : null;
            }
        }

        /// <summary>
        /// The items's Range Key Value
        /// </summary>
        public Object RangeKeyValue
        {
            get
            {
                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.Value).Single() : null;
            }
        }

        /// <summary>
        /// The items's Range Key AttributeValue
        /// </summary>
        internal AttributeValue RangeKeyAttributeValue
        {
            get
            {
                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.ToAttributeValue()).Single() : null;
            }
        }

        /// <summary>
        /// Does the item have a Range Key
        /// </summary>
        internal Boolean HasRangeKey
        {
            get
            {
                return _attributes.Count(x => x.IsRangeKey == true) == 1;
            }
        }

        /// <summary>
        /// The number of Attributes
        /// </summary>
        /// <returns>Int32</returns>
        public Int32 Count
        {
            get
            {
                return _attributes.Count();
            }
        }
        #endregion
    }
}
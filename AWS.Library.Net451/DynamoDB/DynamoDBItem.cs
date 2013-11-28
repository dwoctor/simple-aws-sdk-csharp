using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			try
			{
            	Add(name, value, isHashKey, isRangeKey);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attribute">Attribute to add to the item</param>
        public DynamoDBItem(DynamoDBAttribute attribute)
        {
			try
			{
	            Add(attribute);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        public DynamoDBItem(List<DynamoDBAttribute> attributes)
        {
			try
			{
            	Add(attributes);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        public DynamoDBItem(Dictionary<String, Object> attributes)
        {
			try
			{
            	Add(attributes);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="items">Attributes to add to the item</param>
        internal DynamoDBItem(List<Dictionary<String, AttributeValue>> items)
        {
			try
			{
	            if (items.Count == 1)
	            {
	                Add(items[0]);
	            }
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBItem
        /// </summary>
        /// <param name="attributes">Attributes to add to the item</param>
        internal DynamoDBItem(Dictionary<String, AttributeValue> attributes)
        {
			try
			{
            	Add(attributes);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
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
			try
			{
            	_attributes.Add(new DynamoDBAttribute(name, value, isHashKey, isRangeKey));
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Adds an attribute to the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Add(DynamoDBAttribute attribute)
        {
			try
			{
				_attributes.Add(attribute);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(List<DynamoDBAttribute> attributes)
        {
			try
			{
            	_attributes.AddRange(attributes);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(Dictionary<String, Object> attributes)
        {
			try
			{
            	foreach (var attribute in attributes)
	            {
	                _attributes.Add(new DynamoDBAttribute(attribute.Key, attribute.Value));
	            }
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        internal void Add(Dictionary<String, AttributeValue> attributes)
        {
			try
			{
	            foreach (var attribute in attributes)
		        {
	                _attributes.Add(new DynamoDBAttribute(attribute.Key, attribute.Value));
		        }
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
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
			try
			{
            	_attributes.RemoveAll(x => x.Name == name && x.Value == value && x.IsHashKey == isHashKey && x.IsRangeKey == isRangeKey);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Remove(DynamoDBAttribute attribute)
        {
			try
			{
            	_attributes.Remove(attribute);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }
        #endregion

        /// <summary>
        /// Does an Attribute Exist 
        /// </summary>
        /// <param name="name">The name of the attribute to find</param>
        /// <returns>Boolean</returns>
        public Boolean ContainsAttribute(String name)
        {
			try
			{
            	return _attributes.Any(x => x.Name == name);
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        internal Dictionary<String, List<DynamoDBAttribute>> GetAttributesGroupedByName()
        {
			try
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
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Returns an enumerator that iterates through the AWS.DynamoDB.DynamoDBItem
        /// </summary>
        /// <returns>A AWS.DynamoDB.DynamoDBAttribute.Enumerator for the AWS.DynamoDB.DynamoDBItem</returns>
        public IEnumerator<DynamoDBAttribute> GetEnumerator()
        {
			try
			{
            	return this._attributes.GetEnumerator();
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
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
					try
					{
	                	return _attributes.Single(x => x.Name == name);
					}
					catch(Exception error)
					{
						throw DynamoDBItemException.Generator(error);
					}
	            }
	            set
	            {
					try
					{
		                if (name != null && _attributes.Count(x => x.Name == name) == 0)
		                {
		                    Add(name, value);
		                }
		                else
		                {
							throw DynamoDBItemException.Generator("Invalid Name");
		                }
					}
					catch(Exception error)
					{
						throw DynamoDBItemException.Generator(error);
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
			try
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
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Returns the Hash Code of the DynamoDBItem
        /// </summary>
        /// <returns>Int32</returns>
        public override Int32 GetHashCode()
        {
			try
			{
            	return base.GetHashCode();
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
        }

        /// <summary>
        /// Converts the attribute to an AttributeValue object
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal Dictionary<String, AttributeValue> ToDictionary()
        {
			try
			{
	            Dictionary<String, AttributeValue> dictionary = new Dictionary<String, AttributeValue>();
	            foreach (var attribute in _attributes)
	            {
	                dictionary.Add(attribute.Name, attribute.ToAttributeValue());
	            }
	            return dictionary;
			}
			catch(Exception error)
			{
				throw DynamoDBItemException.Generator(error);
			}
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
				try
				{
	                return _attributes.Select(x => x.Name).ToList();
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The attribute's value
        /// </summary>
        public List<Object> AttributeValues
        {
	        get
			{
				try
				{
	                return _attributes.Select(x => x.Value).ToList();
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's types
        /// </summary>
        public List<Types.Enum> AttributeTypes
        {
			get
			{
				try
				{
	                return _attributes.Select(x => x.Type).ToList();
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Hash Key Value
        /// </summary>
        public DynamoDBAttribute HashKey
        {
			get
			{
				try
				{
                    if (HasHashKey)
                    {
                        return _attributes.Where(x => x.IsHashKey == true).Single();
                    }
                    else
                    {
                        return null;
                    }
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
	        }
			set
			{
				try
				{
                    if(HasHashKey)
                    {
                        _attributes[_attributes.FindIndex(x => x.IsHashKey == true)] = value;
                    }
                    else
                    {
                        _attributes.Add(value);
                    }
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Hash Key Name
        /// </summary>
        public String HashKeyName
        {
			get
			{
				try
				{
	                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.Name).Single() : null;
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Hash Key Value
        /// </summary>
        public Object HashKeyValue
        {
			get
			{
				try
				{
	                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.Value).Single() : null;
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Hash Key AttributeValue
        /// </summary>
        internal AttributeValue HashKeyAttributeValue
        {
			get
			{
				try
	            {
	                return HasHashKey ? _attributes.Where(x => x.IsHashKey == true).Select(y => y.ToAttributeValue()).Single() : null;
				}
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// Does the item have a Hash Key
        /// </summary>
        internal Boolean HasHashKey
        {
			get
			{
				try
				{
	                return _attributes.Count(x => x.IsHashKey == true) == 1;
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Range Key Name
        /// </summary>
        public String RangeKeyName
        {
			get
			{
				try
	            {
	                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.Name).Single() : null;
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Range Key Value
        /// </summary>
        public Object RangeKeyValue
        {
			get
			{
				try
	            {
	                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.Value).Single() : null;
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// The items's Range Key AttributeValue
        /// </summary>
        internal AttributeValue RangeKeyAttributeValue
        {
			get
			{
				try
	            {
	                return HasRangeKey ? _attributes.Where(x => x.IsRangeKey == true).Select(y => y.ToAttributeValue()).Single() : null;
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }

        /// <summary>
        /// Does the item have a Range Key
        /// </summary>
        internal Boolean HasRangeKey
        {
			get
			{
				try
	            {
	                return _attributes.Count(x => x.IsRangeKey == true) == 1;
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
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
				try
	            {
	                return _attributes.Count();
	            }
				catch(Exception error)
				{
					throw DynamoDBItemException.Generator(error);
				}
			}
        }
        #endregion
    }
}
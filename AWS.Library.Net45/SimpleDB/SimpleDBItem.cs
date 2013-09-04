using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.SimpleDB.Model.Internal;
using Amazon.SimpleDB.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWS.SimpleDB
{
    /// <summary>
    /// A SimpleDBItem
    /// </summary>
    public class SimpleDBItem
    {
        #region Fields
        /// <summary>
        /// Stores the item's name
        /// </summary>
        private String _name;

        /// <summary>
        /// Stores the item's attributes
        /// </summary>
        private List<SimpleDBAttribute> _attributes = new List<SimpleDBAttribute>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs an SimpleDBItem
        /// </summary>
        /// <param name="name">The item's name</param>
        public SimpleDBItem(String name)
        {
            _name = name;
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributeName">Attribute's name to add to the item</param>
        /// <param name="attributeValue">Attribute's value to add to the item</param>
        public SimpleDBItem(String itemName, String attributeName, String attributeValue) : this(itemName)
        {
            Add(attributeName, attributeValue);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attribute">Attribute to add to the item</param>
        public SimpleDBItem(String itemName, SimpleDBAttribute attribute) : this(itemName)
        {
            Add(attribute);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        public SimpleDBItem(String itemName, List<SimpleDBAttribute> attributes) : this(itemName)
        {
            Add(attributes);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attribute">Attribute to add to the item</param>
        internal SimpleDBItem(String itemName, Amazon.SimpleDB.Model.Attribute attribute) : this(itemName)
        {
            Add(attribute);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        internal SimpleDBItem(String itemName, List<Amazon.SimpleDB.Model.Attribute> attributes) : this(itemName)
        {
            Add(attributes);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        public SimpleDBItem(String itemName, Dictionary<String, String> attributes) : this(itemName)
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
        public void Add(String name, String value)
        {
            _attributes.Add(new SimpleDBAttribute(name, value));
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(Dictionary<String, String> attributes)
        {
            foreach (var attribute in attributes)
            {
                _attributes.Add(new SimpleDBAttribute(attribute.Key, attribute.Value));
            }
        }

        /// <summary>
        /// Adds an attribute to the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Add(SimpleDBAttribute attribute)
        {
            _attributes.Add(attribute);
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(List<SimpleDBAttribute> attributes)
        {
            _attributes.AddRange(attributes);
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="attribute">The attribute</param>
        internal void Add(Amazon.SimpleDB.Model.Attribute attribute)
        {
            _attributes.Add(new SimpleDBAttribute(attribute.Name, attribute.Value));
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="attributes">The attributes</param>
        internal void Add(List<Amazon.SimpleDB.Model.Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                _attributes.Add(new SimpleDBAttribute(attribute.Name, attribute.Value));
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="name">The attribute's name</param>
        /// <param name="value">The attribute's value</param>
        public void Remove(String name, String value)
        {
            _attributes.RemoveAll(x => x.Name == name && x.Value == value);
        }

        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Remove(SimpleDBAttribute attribute)
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

        /// <summary>
        /// Returns an enumerator that iterates through the AWS.SimpleDB.SimpleDBItem
        /// </summary>
        /// <returns>A AWS.SimpleDB.SimpleDBAttribute.Enumerator for the AWS.SimpleDB.SimpleDBItem</returns>
        public IEnumerator<SimpleDBAttribute> GetEnumerator()
        {
            return this._attributes.GetEnumerator();
        }

        /// <summary>
        /// Get and set the item's attributes
        /// </summary>
        /// <param name="name">The name of the attribute to get or set</param>
        /// <returns>SimpleDBAttribute</returns>
        [System.Runtime.CompilerServices.IndexerName("SimpleDBItemIndex")]
        public SimpleDBAttribute this[String name]
        {
            get
            {
                return _attributes.Single(x => x.Name == name);
            }
            set
            {
                if (name != null && _attributes.Count(x => x.Name == name) == 0)
                {
                    Add(value);
                }
                else
                {
                    throw new Exception("Invalid Name");
                }
            }
        }

        /// <summary>
        /// Converts the attribute to an AttributeValue object
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal List<ReplaceableAttribute> ToListOfReplaceableAttributes()
        {
            List<ReplaceableAttribute> list = new List<ReplaceableAttribute>();
            foreach (var attribute in _attributes)
            {
                list.Add(attribute.ToReplaceableAttribute());
            }
            return list;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The items's Name
        /// </summary>
        public String Name { get { return _name; } set { value = _name; } }

        /// <summary>
        /// The item's attributes
        /// </summary>
        public List<SimpleDBAttribute> Attributes { get { return _attributes; } set { value = _attributes; } }

        /// <summary>
        /// The items's attribute names
        /// </summary>
        public List<String> AttributeNames { get { return _attributes.Select(x => x.Name).ToList(); } }

        /// <summary>
        /// The items's attribute values
        /// </summary>
        public List<String> AttributeValues { get { return _attributes.Select(x => x.Value).ToList(); } }

        /// <summary>
        /// The number of Attributes
        /// </summary>
        /// <returns>Int32</returns>
        public Int32 Count { get { return _attributes.Count(); } }
        #endregion
    }
}
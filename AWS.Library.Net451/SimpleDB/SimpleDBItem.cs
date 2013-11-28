using Amazon.SimpleDB.Model;
using AWS.SimpleDB;
using AWS.SimpleDB.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
            try
            {
                _name = name;
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributeName">Attribute's name to add to the item</param>
        /// <param name="attributeValue">Attribute's value to add to the item</param>
        public SimpleDBItem(String itemName, String attributeName, String attributeValue) : this(itemName)
        {
            try
            {
                Add(attributeName, attributeValue);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attribute">Attribute to add to the item</param>
        public SimpleDBItem(String itemName, SimpleDBAttribute attribute) : this(itemName)
        {
            try
            {
                Add(attribute);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        public SimpleDBItem(String itemName, List<SimpleDBAttribute> attributes) : this(itemName)
        {
            try
            {
                Add(attributes);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attribute">Attribute to add to the item</param>
        internal SimpleDBItem(String itemName, Amazon.SimpleDB.Model.Attribute attribute) : this(itemName)
        {
            try
            {
                Add(attribute);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        internal SimpleDBItem(String itemName, List<Amazon.SimpleDB.Model.Attribute> attributes) : this(itemName)
        {
            try
            {
                Add(attributes);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="itemName">The item's name</param>
        /// <param name="attributes">Attributes to add to the item</param>
        public SimpleDBItem(String itemName, Dictionary<String, String> attributes) : this(itemName)
        {
            try
            {
                Add(attributes);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
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
        public void Add(String name, String value)
        {
            try
            {
                _attributes.Add(new SimpleDBAttribute(name, value));
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(Dictionary<String, String> attributes)
        {
            try
            {
                foreach (var attribute in attributes)
                {
                    _attributes.Add(new SimpleDBAttribute(attribute.Key, attribute.Value));
                }
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Adds an attribute to the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Add(SimpleDBAttribute attribute)
        {
            try
            {
                _attributes.Add(attribute);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Adds attributes to the item
        /// </summary>
        /// <param name="attributes">The attributes</param>
        public void Add(List<SimpleDBAttribute> attributes)
        {
            try
            {
                _attributes.AddRange(attributes);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="attribute">The attribute</param>
        internal void Add(Amazon.SimpleDB.Model.Attribute attribute)
        {
            try
            {
                _attributes.Add(new SimpleDBAttribute(attribute.Name, attribute.Value));
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SimpleDBItem
        /// </summary>
        /// <param name="attributes">The attributes</param>
        internal void Add(List<Amazon.SimpleDB.Model.Attribute> attributes)
        {
            try
            {
                foreach (var attribute in attributes)
                {
                    _attributes.Add(new SimpleDBAttribute(attribute.Name, attribute.Value));
                }
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
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
            try
            {
                _attributes.RemoveAll(x => x.Name == name && x.Value == value);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Remove an attribute from the item
        /// </summary>
        /// <param name="attribute">The attribute</param>
        public void Remove(SimpleDBAttribute attribute)
        {
            try
            {
                _attributes.Remove(attribute);
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
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
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the AWS.SimpleDB.SimpleDBItem
        /// </summary>
        /// <returns>A AWS.SimpleDB.SimpleDBAttribute.Enumerator for the AWS.SimpleDB.SimpleDBItem</returns>
        public IEnumerator<SimpleDBAttribute> GetEnumerator()
        {
            try
            {
                return this._attributes.GetEnumerator();
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
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
                try
                {
                    return _attributes.Single(x => x.Name == name);
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
            set
            {
                try
                {
                    if (name != null && _attributes.Count(x => x.Name == name) == 0)
                    {
                        Add(value);
                    }
                    else
                    {
                        throw SimpleDBItemException.Generator("Invalid Name");
                    }
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
        }

        /// <summary>
        /// Converts the attribute to an AttributeValue object
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal List<ReplaceableAttribute> ToListOfReplaceableAttributes()
        {
            try
            {
                List<ReplaceableAttribute> list = new List<ReplaceableAttribute>();
                foreach (var attribute in _attributes)
                {
                    list.Add(attribute.ToReplaceableAttribute());
                }
                return list;
            }
            catch (Exception error)
            {
                throw SimpleDBItemException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The items's Name
        /// </summary>
        public String Name
        {
            get
            {
                try
                {
                    return _name;
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
            set
            {
                try
                {
                    value = _name;
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The item's attributes
        /// </summary>
        public List<SimpleDBAttribute> Attributes
        {
            get
            {
                try
                {
                    return _attributes;
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
            set
            {
                try
                {
                    value = _attributes;
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The items's attribute names
        /// </summary>
        public List<String> AttributeNames
        {
            get
            {
                try
                {
                    return _attributes.Select(x => x.Name).ToList();
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
        }

        /// <summary>
        /// The items's attribute values
        /// </summary>
        public List<String> AttributeValues
        {
            get
            {
                try
                {
                    return _attributes.Select(x => x.Value).ToList();
                }
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
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
                catch (Exception error)
                {
                    throw SimpleDBItemException.Generator(error);
                }
            }
        }
        #endregion
    }
}
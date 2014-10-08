using Amazon.SimpleDB.Model;
using AWS.SimpleDB;
using AWS.SimpleDB.Exceptions;
using System;

namespace AWS.SimpleDB
{
    /// <summary>
    /// A SimpleDB Attribute
    /// </summary>
    public class SimpleDBAttribute
    {
        #region Constructors
        /// <summary>
        /// Constructs a SimpleDBAttribute
        /// </summary>
        public SimpleDBAttribute() { }

        /// <summary>
        /// Constructs a SimpleDBAttribute
        /// </summary>
        internal SimpleDBAttribute(Amazon.SimpleDB.Model.Attribute attribute) : this(attribute.Name, attribute.Value, false) { }

        /// <summary>
        /// Constructs a SimpleDBAttribute
        /// </summary>
        public SimpleDBAttribute(String name, String value, Boolean isToReplace = false)
        {
            try
            {
                Name = name;
                Value = value;
                IsToReplace = isToReplace;
            }
            catch (Exception error)
            {
                throw SimpleDBAttributeException.Generator(error);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// The attribute's name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The attribute's value
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// The attribute's IsToReplace setting
        /// </summary>
        public Boolean IsToReplace { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Converts the attribute to an Amazon.SimpleDB.Model.ReplaceableAttribute object
        /// </summary>
        /// <returns>ReplaceableAttribute</returns>
        internal Amazon.SimpleDB.Model.ReplaceableAttribute ToReplaceableAttribute()
        {
            try
            {
                return new Amazon.SimpleDB.Model.ReplaceableAttribute { Name = Name, Value = Value, Replace = IsToReplace };
            }
            catch (Exception error)
            {
                throw SimpleDBAttributeException.Generator(error);
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
                if (obj is SimpleDBAttribute)
                {
                    SimpleDBAttribute simpleDBAttributeObj = obj as SimpleDBAttribute;
                    return (this.Name == simpleDBAttributeObj.Name && this.Value == simpleDBAttributeObj.Value);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                throw SimpleDBAttributeException.Generator(error);
            }
        }

        /// <summary>
        /// Returns the Hash Code of the SimpleDBAttribute
        /// </summary>
        /// <returns>Int32</returns>
        public override Int32 GetHashCode()
        {
            try
            {
                return base.GetHashCode();
            }
            catch (Exception error)
            {
                throw SimpleDBAttributeException.Generator(error);
            }
        }

        /// <summary>
        /// Returns the name of the SimpleDBAttribute and it's value
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            try
            {
                return String.Format("{0}, {1}", Name, Value);
            }
            catch (Exception error)
            {
                throw SimpleDBAttributeException.Generator(error);
            }
        }
        #endregion
    }
}
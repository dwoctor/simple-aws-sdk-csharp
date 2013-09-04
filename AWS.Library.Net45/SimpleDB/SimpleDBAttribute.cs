using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.SimpleDB.Model.Internal;
using Amazon.SimpleDB.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public SimpleDBAttribute(String name, String value)
        {
            Name = name;
            Value = value;
            IsToReplace = false;
        }

        /// <summary>
        /// Constructs a SimpleDBAttribute
        /// </summary>
        public SimpleDBAttribute(String name, String value, Boolean isToReplace)
        {
            Name = name;
            Value = value;
            IsToReplace = isToReplace;
        }

        /// <summary>
        /// Constructs a SimpleDBAttribute
        /// </summary>
        public SimpleDBAttribute(Amazon.SimpleDB.Model.Attribute attribute)
        {
            Name = attribute.Name;
            Value = attribute.Value;
            IsToReplace = false;
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
            return new Amazon.SimpleDB.Model.ReplaceableAttribute { Name = Name, Value = Value, Replace = IsToReplace };
        }

        /// <summary>
        /// Compare this object with another object
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Boolean</returns>
        public override Boolean Equals(Object obj)
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

        /// <summary>
        /// Returns the Hash Code of the SimpleDBAttribute
        /// </summary>
        /// <returns>Int32</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the name of the SimpleDBAttribute and it's value
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
            return String.Format("{0}, {1}", Name, Value);
        }
        #endregion
    }
}
using Amazon.DynamoDBv2.Model;
using AWS.DynamoDB;
using AWS.DynamoDB.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace AWS.DynamoDB
{
    /// <summary>
    /// A DynamoDBAttribute
    /// </summary>
    public class DynamoDBAttribute
    {
        #region Fields
        /// <summary>
        /// Stores the attribute's name
        /// </summary>
        private String _name;

        /// <summary>
        /// Stores the attribute's value
        /// </summary>
        private Object _value;

        /// <summary>
        /// Stores the attribute's Type
        /// </summary>
        private Types.Enum _attributeType;

        /// <summary>
        /// Stores a string attribute
        /// </summary>
        private String _attributeString;

        /// <summary>
        /// Stores a number attribute
        /// </summary>
        private String _attributeNumber;

        /// <summary>
        /// Stores a binary attribute
        /// </summary>
        private MemoryStream _attributeBinary;

        /// <summary>
        /// Stores is the attribute a hash key
        /// </summary>
        private Boolean _isHashKey;

        /// <summary>
        /// Stores is the attribute a range key
        /// </summary>
        private Boolean _isRangeKey;
        #endregion

        #region Constructors
        /// <summary>
		/// Constructs a DynamoDBAttribute
        /// </summary>
        public DynamoDBAttribute() { }

        /// <summary>
		/// Constructs a DynamoDBAttribute
        /// </summary>
        /// <param name="name">The attribute's name to add to the item</param>
        /// <param name="value">The attribute's value to add to the item</param>
        /// <param name="isHashKey">Is the Attribute a Hash Key</param>
        /// <param name="isRangeKey">Is the Attribute a Range Key</param>
        public DynamoDBAttribute(String name, Object value, Boolean isHashKey = false, Boolean isRangeKey = false)
        {
			try
			{
	            Name = name;
	            Value = value;
	            IsHashKey = isHashKey;
	            IsRangeKey = isRangeKey;
			}
			catch(Exception error)
			{
				throw DynamoDBAttributeException.Generator(error);
			}
        }

        /// <summary>
		/// Constructs a DynamoDBAttribute
        /// </summary>
        /// <param name="name">The attribute's name to add to the item</param>
        /// <param name="value">The attribute's value to add to the item</param>
        /// <param name="isHashKey">Is the Attribute a Hash Key</param>
        /// <param name="isRangeKey">Is the Attribute a Range Key</param>
        internal DynamoDBAttribute(String name, AttributeValue value, Boolean isHashKey = false, Boolean isRangeKey = false)
        {
			try
			{
	            Name = name;
	            Value = value;
	            IsHashKey = isHashKey;
	            IsRangeKey = isRangeKey;
			}
			catch(Exception error)
			{
				throw DynamoDBAttributeException.Generator(error);
			}
        }
        #endregion

        #region Properties
        /// <summary>
        /// The attribute's name
        /// </summary>
        public String Name
        {
            get
            {
				try
				{
                	return _name;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
            set
            {
				try
				{
                	_name = value;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
        }

        /// <summary>
        /// The attribute's value
        /// </summary>
        public Object Value
        {
            get
            {
				try
				{
                	return _value;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
            set
            {
				try
				{
	                switch (value.GetType().Name)
	                {
	                    case "String":
	                        _attributeType = Types.Enum.String;
	                        _attributeString = value.ToString();
	                        _value = value;
	                        break;
	                    case "Byte":
	                    case "SByte":
	                    case "Int16":
	                    case "UInt16":
	                    case "Int32":
	                    case "UInt32":
	                    case "Int64":
	                    case "UInt64":
	                    case "Single":
	                    case "Double":
	                    case "Decimal":
	                        _attributeType = Types.Enum.Number;
	                        _attributeNumber = value.ToString();
	                        _value = value;
	                        break;
	                    case "Byte[]":
	                        _attributeType = Types.Enum.Binary;
	                        _attributeBinary = new MemoryStream((Byte[])value);
	                        _value = value;
	                        break;
	                    case "Guid":
	                        _attributeType = Types.Enum.Binary;
	                        _attributeBinary = new MemoryStream(((Guid)value).ToByteArray());
	                        _value = value;
	                        break;
	                    case "AttributeValue":
	                        AttributeValue attributeValue = value as AttributeValue;
	                        if (attributeValue.S != null)
	                        {
	                            _value = attributeValue.S;
	                            _attributeType = Types.Enum.String;
	                            _attributeString = attributeValue.S;
	                            break;
	                        }
	                        else if (attributeValue.N != null)
	                        {
	                            try
	                            {
	                                _value = Byte.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = SByte.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = Int16.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = UInt16.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = Int32.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = UInt32.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = Int64.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
	                            try
	                            {
	                                _value = UInt64.Parse(attributeValue.N);
	                                _attributeType = Types.Enum.Number;
	                                _attributeNumber = attributeValue.N;
	                                break;
	                            }
	                            catch { }
								throw DynamoDBAttributeException.Generator(DynamoDBAttributeException.Phases.UnsupportedType);
	                        }
	                        else if (attributeValue.B != null)
	                        {
	                            _value = attributeValue.B.ToArray();
	                            _attributeType = Types.Enum.Binary;
	                            _attributeBinary = attributeValue.B;
	                            break;
	                        }
	                        else
	                        {
								throw DynamoDBAttributeException.Generator(DynamoDBAttributeException.Phases.UnsupportedType);
	                        }
	                    default:
							throw DynamoDBAttributeException.Generator(DynamoDBAttributeException.Phases.UnsupportedType);
	                }
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
        }

        /// <summary>
        /// The attribute's type
        /// </summary>
        public Types.Enum Type
        {
            get
            {
				try
				{
                	return _attributeType;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
        }

        /// <summary>
        /// Gets the attribute's to a Key
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal Dictionary<String, AttributeValue> Key
        {
            get
			{
				try
				{
					return new Dictionary<String, AttributeValue>()
					{
						{
							this.Name,
							this.ToAttributeValue()
						}
					};
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
			}
        }

        /// <summary>
        /// Is the attribute a hash key
        /// </summary>
        public Boolean IsHashKey
        {
            get
            {
				try
				{
                	return _isHashKey;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
            set
            {
				try
				{
	                _isHashKey = value;
	                if (value == true && _isRangeKey == true)
	                {
	                    _isRangeKey = !_isHashKey;
	                }
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
        }

        /// <summary>
        /// Is the attribute a range key
        /// </summary>
        public Boolean IsRangeKey
        {
            get
            {
				try
				{
                	return _isRangeKey;
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
            set
            {
				try
				{
	                _isRangeKey = value;
	                if (value == true && _isHashKey == true)
	                {
	                    _isHashKey = !_isRangeKey;
	                }
				}
				catch(Exception error)
				{
					throw DynamoDBAttributeException.Generator(error);
				}
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts the attribute to an AttributeValue object
        /// </summary>
        /// <returns>AttributeValue</returns>
        internal AttributeValue ToAttributeValue()
        {
			try
			{
	            switch (Value.GetType().Name)
	            {
	                case "String":
	                    return new AttributeValue() { S = _attributeString };
	                case "Byte":
	                case "SByte":
	                case "Int16":
	                case "UInt16":
	                case "Int32":
	                case "UInt32":
	                case "Int64":
	                case "UInt64":
	                case "Single":
	                case "Double":
	                case "Decimal":
	                    return new AttributeValue() { N = _attributeNumber };
	                case "Byte[]":
	                case "Guid":
	                    return new AttributeValue() { B = _attributeBinary };
	                default:
						throw DynamoDBAttributeException.Generator(DynamoDBAttributeException.Phases.UnsupportedType);
	            }
			}
			catch(Exception error)
			{
				throw DynamoDBAttributeException.Generator(error);
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
	            if (obj is DynamoDBAttribute)
	            {
	                DynamoDBAttribute dynamoDBAttributeObj = obj as DynamoDBAttribute;
	                return (this._name == dynamoDBAttributeObj._name && this._value == dynamoDBAttributeObj._value);
	            }
	            else
	            {
	                return false;
	            }
			}
			catch(Exception error)
			{
				throw DynamoDBAttributeException.Generator(error);
			}
        }

        /// <summary>
        /// Returns the Hash Code of the DynamoDBAttribute
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
				throw DynamoDBAttributeException.Generator(error);
			}
        }

        /// <summary>
        /// Returns the name of the DynamoDBAttribute and it's value
        /// </summary>
        /// <returns>String</returns>
        public override String ToString()
        {
			try
			{
	            switch (_attributeType)
	            {
	                case Types.Enum.Binary:
	                    return String.Format("{0}, {1}", _name, BitConverter.ToString(_attributeBinary.ToArray()).Replace("-", ""));
	                case Types.Enum.String:
	                    return String.Format("{0}, '{1}'", _name, Value.ToString());
	                case Types.Enum.Number:
	                    return String.Format("{0}, {1}", _name, Value.ToString());
	            }
	            return String.Format("{0}, {1}", _name, Value.ToString());
			}
			catch(Exception error)
			{
				throw DynamoDBAttributeException.Generator(error);
			}
		}
        #endregion
    }
}
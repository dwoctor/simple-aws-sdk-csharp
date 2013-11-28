using Amazon.SQS.Model;
using AWS.SQS;
using AWS.SQS.Exceptions;
using System;
using System.Collections.Generic;

namespace AWS.SQS
{
    public class SQSMessage
    {
        #region Constructors
        /// <summary>
        /// Constructs a SQSMessage
        /// </summary>
        /// <param name="body"></param>
        public SQSMessage(String body)
        {
            try
            {
                this.Attributes = new Dictionary<String, String>();
                this.Body = body;
                this.MessageID = null;
                this.ReceiptHandle = null;
            }
            catch (Exception error)
            {
                throw SQSMessageException.Generator(error);
            }
        }

        /// <summary>
        /// Constructs a SQSMessage
        /// </summary>
        /// <param name="message"></param>
        internal SQSMessage(Message message)
        {
            try
            {
                this.Attributes = message.Attributes;
                this.Body = message.Body;
                this.MessageID = message.MessageId;
                this.ReceiptHandle = message.ReceiptHandle;
            }
            catch (Exception error)
            {
                throw SQSMessageException.Generator(error);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Body of the Message
        /// </summary>
        internal Dictionary<String, String> Attributes { get; set; }

        /// <summary>
        /// The Body of the Message
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// The Message's ID
        /// </summary>
        internal String MessageID { get; set; }

        /// <summary>
        /// The Receipt Handle of the Message
        /// </summary>
        internal String ReceiptHandle { get; set; }

        /// <summary>
        /// Converts to a Message
        /// </summary>
        internal Message ToMessage()
        {
            try
            {
                Message message = new Message();
                if (this.Attributes.Count > 0)
                {
                    message.Attributes = this.Attributes;
                }
                if (this.Body != null)
                {
                    message.Body = this.Body;
                }
                if (this.MessageID != null)
                {
                    message.MessageId = this.MessageID;
                }
                if (this.ReceiptHandle != null)
                {
                    message.ReceiptHandle = message.ReceiptHandle;
                }
                return message;
            }
            catch (Exception error)
            {
                throw SQSMessageException.Generator(error);
            }
        }
        #endregion
    }
}
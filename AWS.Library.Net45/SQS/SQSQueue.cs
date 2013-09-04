using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SQS.Model.Internal;
using Amazon.SQS.Model.Internal.MarshallTransformations;
using Amazon.SQS.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace AWS.SQS
{
    public class SQSQueue
    {
        #region Fields
        /// <summary>
        /// AmazonSQS Client
        /// </summary>
        private AmazonSQSClient _sqs;

        private Message _message = null;
        private List<Message> _messages = new List<Message>();

        private String _messageRecieptHandle = "";
        private List<String> _messageRecieptHandles = new List<String>();
        private String _queueUrl = "";
        #endregion Fields

        /// <summary>
        /// Upon Construction of the Queue
        /// </summary>
        public SQSQueue(String accesskey, String secretAccessKey, String queueurl)
        {
            _sqs = new AmazonSQSClient(accesskey, secretAccessKey);
            _queueUrl = queueurl;
        }

        #region Methods
        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="messagebody">The message to be sent</param>
        public void SendMessage(String messageBody)
        {
            try
            {
                _sqs.SendMessage(new SendMessageRequest() { MessageBody = messageBody, QueueUrl = _queueUrl });
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: SendMessage (Error) {0}", error.Message));
            }
        }

        /// <summary>
        /// Sends multiple messages
        /// </summary>
        /// <param name="messagebodys">The messages to be sent</param>
        public void SendMessages(List<String> messagebodys)
        {
            try
            {
                List<List<SendMessageBatchRequestEntry>> batchs = new List<List<SendMessageBatchRequestEntry>>();
                List<Int32> _writeBatchIndexRequestCounts = new List<Int32>();
                Int32 _batchIndex = 0;
                batchs.Add(new List<SendMessageBatchRequestEntry>());
                for (Int32 i = 0; i < messagebodys.Count; i++)
                {
                    if (batchs[_batchIndex].Count == 10)
                    {
                        _batchIndex++;
                        batchs.Add(new List<SendMessageBatchRequestEntry>());
                    }
                    batchs[_batchIndex].Add(new SendMessageBatchRequestEntry { Id = (batchs[_batchIndex].Count + 1).ToString(), MessageBody = messagebodys[i] });
                }
                foreach (var batch in batchs)
                {
                    _sqs.SendMessageBatch(new SendMessageBatchRequest() { Entries = batch, QueueUrl = _queueUrl });
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: SendMessages (Error) {0}", error.Message));
            }
        }

        public String ReceiveMessageBody()
        {
            _message = ReceiveMessage();
            return (_message == null) ? null : _message.Body;
        }

        public List<String> ReceiveMessageBodys()
        {
            try
            {
                List<String> messagesBodys = new List<String>();
                _messages = ReceiveMessages();
                foreach (var message in _messages)
                {
                    messagesBodys.Add(message.Body);
                }
                return messagesBodys;
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: ReceiveMessageBodys (Error) {0}", error.Message));
            }
            return null;
        }

        public Message ReceiveMessage()
        {
            try
            {
                ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.QueueUrl = _queueUrl;
                ReceiveMessageResponse receiveMessageResponse = _sqs.ReceiveMessage(receiveMessageRequest);
                if (receiveMessageResponse.Messages.Count == 1)
                {
                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        return message;
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: ReceiveMessage (Error) {0}", error.Message));
            }
            return null;
        }

        public List<Message> ReceiveMessages()
        {
            try
            {
                List<Message> messages = new List<Message>();
                Boolean empty = false;
                do
                {
                    try
                    {
                        ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
                        receiveMessageRequest.QueueUrl = _queueUrl;
                        ReceiveMessageResponse receiveMessageResponse = _sqs.ReceiveMessage(receiveMessageRequest);
                        if (receiveMessageResponse.Messages.Count > 0)
                        {
                            foreach (Message message in receiveMessageResponse.Messages)
                            {
                                messages.Add(message);
                            }
                        }
                        else
                        {
                            empty = true;
                        }
                    }
                    catch { }
                }
                while (empty == false);
                return messages;
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: ReceiveMessages (Error) {0}", error.Message));
            }
            return null;
        }

        public void DeleteMessage()
        {
            DeleteMessage(_message);
        }

        public void DeleteMessage(Message message)
        {
            try
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
                deleteMessageRequest.QueueUrl = _queueUrl;
                deleteMessageRequest.ReceiptHandle = message.ReceiptHandle;
                _sqs.DeleteMessage(deleteMessageRequest);
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: DeleteMessage (Error) {0}", error.Message));
            }
        }

        public void DeleteMessages()
        {
            DeleteMessages(_messages);
        }

        public void DeleteMessages(List<Message> messages)
        {
            try
            {
                Int32 currentMessage = 0;
                for (int i = 0; i < messages.Count / 10; i++)
                {
                    DeleteMessageBatchRequest deleteMessageBatchRequest = new DeleteMessageBatchRequest();
                    deleteMessageBatchRequest.QueueUrl = _queueUrl;
                    for (int j = 0; j < 10; j++)
                    {
                        deleteMessageBatchRequest.Entries.Add(new DeleteMessageBatchRequestEntry { Id = messages[currentMessage].MessageId, ReceiptHandle = messages[currentMessage].ReceiptHandle });
                        currentMessage++;
                    }
                    _sqs.DeleteMessageBatch(deleteMessageBatchRequest);
                }
                messages.Clear();
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: DeleteMessages (Error) {0}", error.Message));
            }
        }

        public void ClearQueue()
        {
            DeleteMessages(ReceiveMessages());
        }
        #endregion
    }
}
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace AWS.SQS
{
    public class SQS
    {
        #region Fields
        /// <summary>
        /// AmazonSQS Client
        /// </summary>
        private AmazonSQS _sqs;

        private Message _message = null;
        private List<Message> _messages = new List<Message>();

        private String _messageRecieptHandle = "";
        private List<String> _messageRecieptHandles = new List<String>();
        private String _queueUrl = "";
        #endregion Fields

        /// <summary>
        /// Upon Construction of the Queue
        /// </summary>
        public SQS(String accesskey, String secretAccessKey, String queueurl)
        {
            _sqs = new AmazonSQSClient(accesskey, secretAccessKey);
            _queueUrl = queueurl;
        }

        #region Methods
        public void SendMessage(String messageBody)
        {
            try
            {
                SendMessageRequest sendMessageRequest = new SendMessageRequest();
                sendMessageRequest.QueueUrl = _queueUrl;
                sendMessageRequest.MessageBody = messageBody;
                _sqs.SendMessage(sendMessageRequest);
            }
            catch (Exception error)
            {
                Debug.WriteLine(String.Format("SQS: SendMessage (Error) {0}", error.Message));
            }
        }

        public void SendMessages(List<String> messagebodys)
        {
            try
            {
                Int32 currentMessage = 0;
                for (int i = 0; i < messagebodys.Count / 10; i++)
                {
                    SendMessageBatchRequest sendMessageBatchRequest = new SendMessageBatchRequest();
                    sendMessageBatchRequest.QueueUrl = _queueUrl;
                    for (int j = 0; j < 10; j++)
                    {
                        sendMessageBatchRequest.Entries.Add(new SendMessageBatchRequestEntry { Id = j.ToString(), MessageBody = messagebodys[currentMessage++] });
                    }
                    if (sendMessageBatchRequest.Entries.Count > 0)
                    {
                        _sqs.SendMessageBatch(sendMessageBatchRequest);
                    }
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
                if (receiveMessageResponse.IsSetReceiveMessageResult())
                {
                    ReceiveMessageResult receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                    foreach (Message message in receiveMessageResult.Message)
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
                        if (receiveMessageResponse.IsSetReceiveMessageResult())
                        {
                            ReceiveMessageResult receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                            if (receiveMessageResult.Message.Count > 0)
                            {
                                foreach (Message message in receiveMessageResult.Message)
                                {
                                    messages.Add(message);
                                }
                            }
                            else
                            {
                                empty = true;
                            }
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
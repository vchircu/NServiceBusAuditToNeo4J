using System;

namespace NServiceBus.Model
{
    using NServiceBus.Model.Domain;

    public class Mapper
    {
        private string contextHeaderName;
        private Func<string, string> messageTypeTransformerFunc;

        public Mapper WithContextHeaderName(string headerName)
        {
            contextHeaderName = headerName;
            return this;
        }

        public Mapper WithMessageTypeTransformer(Func<string, string> messageTypeTransformer)
        {
            messageTypeTransformerFunc = messageTypeTransformer; 
            return this;
        }

        public ProcessedMessage Map(Data.ProcessedMessage dataMessage)
        {
            return new ProcessedMessage
                       {
                           IsSystemMessage = dataMessage.MessageMetadata.IsSystemMessage, 
                           MessageType = GetMessageType(dataMessage.MessageMetadata.MessageType), 
                           MessageId = dataMessage.MessageMetadata.MessageId, 
                           RelatedTo = dataMessage.MessageMetadata.RelatedToId, 
                           MessageIntent = GetMessageIntent(dataMessage), 
                           Context = GetContext(dataMessage), 
                           ReceivingEndpoint = Map(dataMessage.MessageMetadata.ReceivingEndpoint), 
                           SendingEndpoint = Map(dataMessage.MessageMetadata.SendingEndpoint)
                       };
        }

        private string GetMessageType(string messageType)
        {
            if (messageTypeTransformerFunc == null)
            {
                return messageType;
            }

            return string.IsNullOrEmpty(messageType) ? messageType : messageTypeTransformerFunc(messageType);
        }

        private static string GetMessageIntent(Data.ProcessedMessage dataMessage)
        {
            if (dataMessage?.Headers == null)
            {
                return string.Empty;
            }

            return !dataMessage.Headers.TryGetValue("NServiceBus.MessageIntent", out var intent) ? string.Empty : intent;
        }

        private static Endpoint Map(Data.Endpoint dataEndpoint)
        {
            return dataEndpoint != null ? new Endpoint {Name = dataEndpoint.Name} : null;
        }

        private string GetContext(Data.ProcessedMessage dataMessage)
        {
            dataMessage.Headers.TryGetValue(contextHeaderName, out var headerValue);

            return headerValue;
        }
    }
}
namespace NServiceBus.Model
{
    using NServiceBus.Model.Domain;

    public class Mapper
    {
        private readonly string contextHeaderName;

        public Mapper(string contextHeaderName)
        {
            this.contextHeaderName = contextHeaderName;
        }

        public ProcessedMessage Map(Data.ProcessedMessage dataMessage)
        {
            return new ProcessedMessage
                       {
                           IsSystemMessage = dataMessage.MessageMetadata.IsSystemMessage, 
                           MessageType = dataMessage.MessageMetadata.MessageType, 
                           MessageId = dataMessage.MessageMetadata.MessageId, 
                           RelatedTo = dataMessage.MessageMetadata.RelatedToId, 
                           MessageIntent = GetMessageIntent(dataMessage), 
                           Context = GetContext(dataMessage), 
                           ReceivingEndpoint = Map(dataMessage.MessageMetadata.ReceivingEndpoint), 
                           SendingEndpoint = Map(dataMessage.MessageMetadata.SendingEndpoint)
                       };
        }

        private static string GetMessageIntent(Data.ProcessedMessage dataMessage)
        {
            if (dataMessage?.Headers == null)
            {
                return string.Empty;
            }

            string intent;
            if (!dataMessage.Headers.TryGetValue("NServiceBus.MessageIntent", out intent))
            {
                return string.Empty;
            }

            return intent;
        }

        private static Endpoint Map(Data.Endpoint dataEndpoint)
        {
            return dataEndpoint != null ? new Endpoint {Name = dataEndpoint.Name} : null;
        }

        private string GetContext(Data.ProcessedMessage dataMessage)
        {
            string headerValue;
            dataMessage.Headers.TryGetValue(contextHeaderName, out headerValue);

            return headerValue;
        }
    }
}
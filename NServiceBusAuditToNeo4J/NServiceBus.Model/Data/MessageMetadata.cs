namespace NServiceBus.Model.Data
{
    using System;

    public class MessageMetadata
    {
        public Guid ConversationId { get; set; }

        public bool IsSystemMessage { get; set; }

        public string MessageId { get; set; }

        public string MessageType { get; set; }

        public Endpoint ReceivingEndpoint { get; set; }

        public string RelatedToId { get; set; }

        public Endpoint SendingEndpoint { get; set; }
    }
}
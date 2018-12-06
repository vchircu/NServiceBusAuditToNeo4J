namespace NServiceBus.Model.Data
{
    using System;

    public class MessageMetadata
    {
        public Guid ConversationId { get; set; }

        public bool IsSystemMessage { get; set; }

        public Guid MessageId { get; set; }

        public string MessageType { get; set; }

        public Endpoint ReceivingEndpoint { get; set; }

        public Guid? RelatedToId { get; set; }

        public Endpoint SendingEndpoint { get; set; }
    }
}
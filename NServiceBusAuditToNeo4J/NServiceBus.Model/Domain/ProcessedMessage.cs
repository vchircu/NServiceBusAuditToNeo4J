namespace NServiceBus.Model.Domain
{
    using System;

    public class ProcessedMessage
    {
        public Guid MessageId { get; set; }

        public string MessageType { get; set; }

        public string MessageIntent { get; set; }

        public string Context { get; set; }

        public Endpoint ReceivingEndpoint { get; set; }

        public Guid? RelatedTo { get; set; }

        public Endpoint SendingEndpoint { get; set; }

        public bool IsSystemMessage { get; set; }
    }
}
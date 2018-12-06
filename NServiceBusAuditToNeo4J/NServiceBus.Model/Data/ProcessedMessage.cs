namespace NServiceBus.Model.Data
{
    using System.Collections.Generic;

    public class ProcessedMessage
    {
        public Dictionary<string, string> Headers { get; set; }

        public MessageMetadata MessageMetadata { get; set; }
    }
}
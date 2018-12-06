namespace ModelBuilder
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class Endpoint
    {
        private readonly ConcurrentBag<string> sentMessages = new ConcurrentBag<string>();

        private readonly ConcurrentBag<string> receivedMessages = new ConcurrentBag<string>();

        public string Name { get; set; }

        public IEnumerable<string> MessagesSent => sentMessages;

        public IEnumerable<string> MessagesReceived => receivedMessages;

        internal void Sends(InterimMessage interimMessage)
        {
            sentMessages.Add(interimMessage.Type);
        }

        internal void Receives(InterimMessage interimMessage)
        {
            receivedMessages.Add(interimMessage.Type);
        }
    }
}
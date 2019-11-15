namespace ModelBuilder
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class Model
    {
        private readonly ConcurrentDictionary<string, InterimMessage> interimMessages =
            new ConcurrentDictionary<string, InterimMessage>();

        private readonly ConcurrentDictionary<string, Message> messages = new ConcurrentDictionary<string, Message>();

        private readonly ConcurrentDictionary<string, Endpoint> endpoints = new ConcurrentDictionary<string, Endpoint>();

        private readonly ConcurrentDictionary<string, Context> contexts = new ConcurrentDictionary<string, Context>();

        public Endpoint GetEndpoint(string name)
        {
            return endpoints.GetOrAdd(name, id => new Endpoint { Name = name });
        }

        public Context GetContext(string context)
        {
            return contexts.GetOrAdd(context, id => new Context { Name = context });
        }

        public IEnumerable<Message> GetMessages()
        {
            return messages.Values;
        }

        public IEnumerable<Endpoint> GetEndpoints()
        {
            return endpoints.Values;
        }

        public IEnumerable<Context> GetContexts()
        {
            return contexts.Values;
        }

        internal void Compact()
        {
            foreach (KeyValuePair<string, InterimMessage> message in interimMessages)
            {
                var messageNode = GetMessage(message.Value.Type);
                messageNode.Intent = message.Value.Intent;

                InterimMessage relatedMessage;
                if (!string.IsNullOrEmpty(message.Value.RelatedTo) && interimMessages.TryGetValue(message.Value.RelatedTo, out relatedMessage))
                {
                    messageNode.AddRelatedTo(relatedMessage.Type);
                }
            }
        }

        internal InterimMessage GetInterimMessage(string messageId)
        {
            return interimMessages.GetOrAdd(messageId, id => new InterimMessage());
        }

        private Message GetMessage(string id)
        {
            return messages.GetOrAdd(id, key => new Message { Id = id });
        }
    }
}
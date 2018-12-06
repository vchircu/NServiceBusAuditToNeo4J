namespace RavenDBExporter
{
    using System;
    using System.Collections.Generic;

    using Exporter;

    using NServiceBus.Model.Data;

    using Raven.Abstractions.Data;
    using Raven.Client;

    public class MessageProcessor : IMessageProcessor
    {
        private readonly IDocumentStore documentStore;

        public MessageProcessor(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public void RegisterListener(Action<ProcessedMessage> onNext)
        {
            using (var session = this.documentStore.OpenSession())
            using (
                IEnumerator<StreamResult<ProcessedMessage>> stream =
                    session.Advanced.Stream<ProcessedMessage>("ProcessedMessage"))
            {
                while (stream.MoveNext())
                {
                    onNext(stream.Current.Document);
                }
            }
        }
    }
}
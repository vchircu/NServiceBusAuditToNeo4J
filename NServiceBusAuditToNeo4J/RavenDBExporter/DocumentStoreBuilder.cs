namespace RavenDBExporter
{
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Imports.Newtonsoft.Json;

    public class DocumentStoreBuilder
    {
        public static IDocumentStore GetDocumentStore(string url)
        {
            var store = new DocumentStore
                            {
                                Url = url, 
                                Conventions =
                                    {
                                        // Prevents $type from interfering with deserialization of EndpointDetails
                                        CustomizeJsonSerializer =
                                            serializer =>
                                            serializer.TypeNameHandling = TypeNameHandling.None
                                    }, 
                            };

            store.Initialize();

            return store;
        }
    }
}
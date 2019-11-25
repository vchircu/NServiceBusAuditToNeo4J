namespace RouteVisualisation
{
    using System.Configuration;
    using System.Threading.Tasks;
    using Exporter;
    using Importer;
    using ModelBuilder;
    using Neo4jImporter;
    using NServiceBus.Model;
    using RavenDBExporter;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static Task MainAsync()
        {
            var serviceControlDbUrl = ConfigurationManager.AppSettings["ServiceControl/RavenAddress"];

            IMessageProcessor exporter = GetMessageProcessor(serviceControlDbUrl);

            var modelBuilder = new InMemoryModelBuilder();

            var mapper = new Mapper()
                .WithContextHeaderName(ConfigurationManager.AppSettings["ContextHeaderName"])
                .WithMessageTypeTransformer(t => t.Replace("MyShop.", string.Empty).Replace("Messages.", string.Empty));

            exporter.RegisterListener(dataMessage => modelBuilder.Accept(mapper.Map(dataMessage)));

            var model = modelBuilder.GetModel();

            var neo4JConfiguration = new Neo4jConfiguration
            {
                User = ConfigurationManager.AppSettings["Neo4j/User"],
                Password =
                    ConfigurationManager.AppSettings["Neo4j/Password"],
                Url = ConfigurationManager.AppSettings["Neo4j/Url"]
            };

            IModelImporter graphImporter = GetGraphModelImporter(neo4JConfiguration);

            return graphImporter.ImportAsync(model);
        }

        private static GraphModelCsvImporter GetGraphModelImporter(Neo4jConfiguration neo4JConfiguration)
        {
            var graphClient = GraphClientBuilder.GetGraphClient(neo4JConfiguration);
            var graphImporter = new GraphModelCsvImporter(
                graphClient,
                ConfigurationManager.AppSettings["Neo4j/ImportLocation"]);
            return graphImporter;
        }

        private static MessageProcessor GetMessageProcessor(string serviceControlDbUrl)
        {
            var documentStore = DocumentStoreBuilder.GetDocumentStore(serviceControlDbUrl);
            var messageProcessor = new MessageProcessor(documentStore);
            return messageProcessor;
        }
    }
}
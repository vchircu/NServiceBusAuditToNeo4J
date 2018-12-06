namespace Neo4jImporter
{
    using System;
    using System.Threading.Tasks;

    using Importer;

    using ModelBuilder;

    using Neo4jClient;

    public class GraphModelCsvImporter : IModelImporter
    {
        private readonly IGraphClient graphClient;

        private readonly string basePath;

        public GraphModelCsvImporter(IGraphClient graphClient, string basePath)
        {
            this.basePath = basePath;
            this.graphClient = graphClient;
        }

        public Task ImportAsync(Model model)
        {
            new CsvFileWriter(basePath).WriteCsvFile(model);
            return LoadCsvIntoDbAsync();
        }

        private static Uri GetFileUri(string fileName)
        {
            const string FileProtocol = "file:///";
            var fileUri = new Uri(FileProtocol + fileName);
            return fileUri;
        }

        private async Task LoadCsvIntoDbAsync()
        {
            await LoadNodes().ConfigureAwait(false);
            await LoadRelationships().ConfigureAwait(false);
        }

        private async Task LoadRelationships()
        {
            await LoadRelatedToRelationships().ConfigureAwait(false);
            await LoadReceivesRelationships().ConfigureAwait(false);
            await LoadSendsRelationships().ConfigureAwait(false);
            await LoadContainsRelationships().ConfigureAwait(false);
        }

        private Task LoadNodes()
        {
            return Task.WhenAll(
                LoadMessageNodes(), 
                LoadEndpointNodes(), 
                LoadContextNodes());
        }

        private Task LoadMessageNodes()
        {
            var fileUri = GetFileUri(FileNames.MessageNodes);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Create("(:Message {id: row.id, intent: row.intent})")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadEndpointNodes()
        {
            var fileUri = GetFileUri(FileNames.EndpointNodes);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Create("(:Endpoint {id: row.id})")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadContextNodes()
        {
            var fileUri = GetFileUri(FileNames.ContextNodes);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Create("(:Context {id: row.id})")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadRelatedToRelationships()
        {
            var fileUri = GetFileUri(FileNames.RelatedToRelationships);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Match("(m:Message {id: row.messageId})")
                    .Match("(related:Message {id: row.relatedMessageId})")
                    .Merge("(m)-[:RELATED_TO]->(related)")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadSendsRelationships()
        {
            var fileUri = GetFileUri(FileNames.MessagesSentRelationships);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Match("(e:Endpoint {id: row.endpointId})")
                    .Match("(m:Message {id: row.messageId})")
                    .Merge("(e)-[:SENDS]->(m)")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadReceivesRelationships()
        {
            var fileUri = GetFileUri(FileNames.MessagesReceivedRelationships);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Match("(e:Endpoint {id: row.endpointId})")
                    .Match("(m:Message {id: row.messageId})")
                    .Merge("(e)-[:RECEIVES]->(m)")
                    .ExecuteWithoutResultsAsync();
        }

        private Task LoadContainsRelationships()
        {
            var fileUri = GetFileUri(FileNames.ContextRelationships);
            return
                graphClient.Cypher.LoadCsv(fileUri, "row", true)
                    .Match("(c:Context {id: row.contextId})")
                    .Match("(m:Message {id: row.messageId})")
                    .Merge("(c)-[:CONTAINS]->(m)")
                    .ExecuteWithoutResultsAsync();
        }
    }
}
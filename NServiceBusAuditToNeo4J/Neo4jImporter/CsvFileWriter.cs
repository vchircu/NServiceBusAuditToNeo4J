namespace Neo4jImporter
{
    using System;
    using System.IO;
    using System.Text;

    using ModelBuilder;

    public class CsvFileWriter
    {
        private readonly string basePath;

        public CsvFileWriter(string basePath)
        {
            this.basePath = basePath;
        }

        public void WriteCsvFile(Model model)
        {
            BuildMessageFile(model);
            BuildEndpointFile(model);
            BuildContextNodesFile(model);
        }

        private void BuildMessageFile(Model model)
        {
            var messageNodes = new StringBuilder();
            var relatedToRelationShips = new StringBuilder();
            foreach (var message in model.GetMessages())
            {
                foreach (var relatedToMessage in message.RelatedToMessages)
                {
                    relatedToRelationShips.AppendFormat(
                        "{0},{1}{2}",
                        message.Id,
                        relatedToMessage,
                        Environment.NewLine);
                }

                messageNodes.AppendFormat("{0},{1}{2}", message.Id, message.Intent, Environment.NewLine);
            }

            WriteCsv(FileNames.MessageNodes, "id,intent", messageNodes);
            WriteCsv(FileNames.RelatedToRelationships, "messageId,relatedMessageId", relatedToRelationShips);
        }

        private void BuildEndpointFile(Model model)
        {
            var endpoints = new StringBuilder();
            var messagesReceivedRelationships = new StringBuilder();
            var messagesSendRelationships = new StringBuilder();
            foreach (var endpoint in model.GetEndpoints())
            {
                endpoints.AppendFormat("{0}{1}", endpoint.Name, Environment.NewLine);

                foreach (var messageReceived in endpoint.MessagesReceived)
                {
                    messagesReceivedRelationships.AppendFormat(
                        "{0},{1}{2}", 
                        endpoint.Name, 
                        messageReceived, 
                        Environment.NewLine);
                }

                foreach (var messagesSent in endpoint.MessagesSent)
                {
                    messagesSendRelationships.AppendFormat(
                        "{0},{1}{2}", 
                        endpoint.Name, 
                        messagesSent, 
                        Environment.NewLine);
                }
            }

            WriteCsv(FileNames.MessagesSentRelationships, "endpointId,messageId", messagesSendRelationships);
            WriteCsv(FileNames.MessagesReceivedRelationships, "endpointId,messageId", messagesReceivedRelationships);
            WriteCsv(FileNames.EndpointNodes, "id", endpoints);
        }

        private void BuildContextNodesFile(Model model)
        {
            var contextNodes = new StringBuilder();
            var contextRelationships = new StringBuilder();
            foreach (var context in model.GetContexts())
            {
                contextNodes.AppendFormat("{0}{1}", context.Name, Environment.NewLine);

                foreach (var containedMessage in context.ContainedMessage)
                {
                    contextRelationships.AppendFormat("{0},{1}{2}", context.Name, containedMessage, Environment.NewLine);
                }
            }

            WriteCsv(FileNames.ContextNodes, "id", contextNodes);
            WriteCsv(FileNames.ContextRelationships, "contextId,messageId", contextRelationships);
        }

        private void WriteCsv(string fileName, string header, StringBuilder text)
        {
            var filePath = Path.Combine(basePath, fileName);
            File.WriteAllText(filePath, header + Environment.NewLine);
            File.AppendAllText(filePath, text.ToString());
        }
    }
}
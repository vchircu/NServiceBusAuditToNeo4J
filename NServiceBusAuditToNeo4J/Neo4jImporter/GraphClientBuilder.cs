namespace Neo4jImporter
{
    using System;

    using Neo4jClient;

    public class GraphClientBuilder
    {
        public static IGraphClient GetGraphClient(Neo4jConfiguration configuration)
        {
            IGraphClient graphClient = new GraphClient(new Uri(configuration.Url), configuration.User, configuration.Password);
            graphClient.Connect();

            return graphClient;
        } 
    }
}
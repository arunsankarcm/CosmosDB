using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace CosmosGremlinDemo.Services
{
    public static class GremlinClientFactory
    {
        public static GremlinClient Create()
        {
            var hostname = "bnv2.gremlin.cosmos.azure.com";
            var port = 443;
            var authKey = "";
            var database = "relationships";
            var container = "playerConnections";

            var gremlinServer = new GremlinServer(
                hostname, port, enableSsl: true,
                username: $"/dbs/{database}/colls/{container}",
                password: authKey
            );

            var serializer = new GraphSON2MessageSerializer();

            return new GremlinClient(gremlinServer, serializer);
        }
    }

}





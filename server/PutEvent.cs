using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace server.Function
{
    public static class PutEvent
    {
        [FunctionName("PutEvent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "event/{id}")] HttpRequest req,
            ILogger log)
        {
            string json = await new StreamReader(req.Body).ReadToEndAsync();
            Event _event = JsonConvert.DeserializeObject<Event>(json);
            var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

            //optie moet je aanmaken door howest
            CosmosClientOptions options = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Gateway
            };

            CosmosClient client = new CosmosClient(connectionString, options);
            var container = client.GetContainer(General.COSMOS_DATABASE, General.COSMOS_CONTAINER_EVENTS);
            await container.UpsertItemAsync<Event>(_event, new PartitionKey(_event.Id));

            return new OkObjectResult(json);
        }
    }
}

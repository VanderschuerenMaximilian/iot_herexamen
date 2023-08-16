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
using server.Models;

namespace server.Function
{
    public static class GetRegistrations
    {
        [FunctionName("GetRegistrations")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "registrations")] HttpRequest req,
            ILogger log)
        {
            string json = await new StreamReader(req.Body).ReadToEndAsync();
            Registation registration = JsonConvert.DeserializeObject<Registation>(json);
            var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

            //optie moet je aanmaken door howest
            CosmosClientOptions options = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Gateway
            };

            CosmosClient client = new CosmosClient(connectionString, options);
            var container = client.GetContainer(General.COSMOS_DATABASE, General.COSMOS_CONTAINER_REGISTRATIONS);
            registration.Id = Guid.NewGuid().ToString();
            registration.ArriveTime = DateTime.Now;
            registration.BandId = Guid.NewGuid().ToString();
            await container.CreateItemAsync<Registation>(registration, new PartitionKey(registration.EventId));

            return new OkObjectResult(json);
        }
    }

    public static class GetRegistrationsByEvent
    {
        [FunctionName("GetRegistrationsByEvent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "registrations/{eventId}")] HttpRequest req,
            ILogger log, string eventId)
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDb");

            //optie moet je aanmaken door howest
            CosmosClientOptions options = new CosmosClientOptions()
            {
                ConnectionMode = ConnectionMode.Gateway
            };

            CosmosClient client = new CosmosClient(connectionString, options);
            var container = client.GetContainer(General.COSMOS_DATABASE, General.COSMOS_CONTAINER_REGISTRATIONS);
            var query = new QueryDefinition("SELECT * FROM c WHERE c.eventId = @eventId")
                .WithParameter("@eventId", eventId);
            var registrations = container.GetItemQueryIterator<Registation>(query);
            var results = await registrations.ReadNextAsync();

            return new OkObjectResult(results);
        }
    }

}

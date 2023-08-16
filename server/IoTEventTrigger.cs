using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Win32;
using Microsoft.Azure.Devices;

namespace server.Function
{
    public static class IoTEventTrigger
    {
        [FunctionName("IoTEventTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "setEvent/{deviceId}/{isActive}")] HttpRequest req, string deviceId, bool isActive,
            ILogger log)
        {
            var iothubadmin = Environment.GetEnvironmentVariable("IoTAdmin");
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(iothubadmin);
            var twin = await registryManager.GetTwinAsync(deviceId);
            twin.Properties.Desired["isActive"] = isActive;
            await registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
            return new OkObjectResult($"Device {deviceId} is set to {isActive}");
        }
    }
}

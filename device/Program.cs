using System.Text;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using device.Models;
using DotNetty.Transport.Bootstrapping;
using System.Runtime.InteropServices;
using System.Security;
using System.ComponentModel;

DesiredProperties desiredProperties = new DesiredProperties();

Registation Registation = new Registation() {
    Name = "Maximilian",
    BandId = Guid.NewGuid(),
    Age = 21,
    Arrive_time = DateTime.Now,
    EventId = "4086056f-c74d-453c-8a90-ce653f610c96"
};

var connectionString = "HostName=MaximilianIoT.azure-devices.net;DeviceId=herexamen;SharedAccessKey=YTCEKX6cwW78kA7hPPMTpbyGQWLMH/1f8gXTaAgaJKs=";
using var deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
// await deviceClient.SetReceiveMessageHandlerAsync(ReceiveMessage, null);
await deviceClient.OpenAsync();
await GetTwinConfigInfo();

#region Boot
async Task GetTwinConfigInfo()
{
    var twin = await deviceClient.GetTwinAsync();
    var twinCollection = twin.Properties.Desired;
    var twinjson = twinCollection.ToJson();
    desiredProperties = JsonConvert.DeserializeObject<DesiredProperties>(twinjson);
    Console.WriteLine("Het event is: " + desiredProperties.IsActive);
}
#endregion

while (true)
{
    Console.WriteLine("Maak uw keuze: ");
    Console.WriteLine("1. Persoon komt binnen op het event");
    Console.WriteLine("2. Persoon verlaat het event");
    Console.WriteLine("9. Afsluiten");
    string keuze = Console.ReadLine();
    Console.WriteLine("U heeft gekozen voor: " + keuze);
    await HandleChoice(keuze);
    Thread.Sleep(1000);
}

async Task HandleChoice(string choice) {
    switch (choice) {
        case "1":
            Console.WriteLine("Persoon komt binnen op het event");
            await SendPersonEnters();
            break;
        case "2":
            Console.WriteLine("Persoon verlaat het event");
            await SendPersonLeaves();
            break;
        case "9":
            Console.WriteLine("Afsluiten");
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Ongeldige keuze");
            break;
    }
}

async Task SendPersonEnters() {
    var messageString = JsonConvert.SerializeObject(Registation);
    var message = new Message(Encoding.ASCII.GetBytes(messageString));
    Console.WriteLine("Sending message: " + messageString);
    await deviceClient.SendEventAsync(message);
}

async Task SendPersonLeaves() {
    Registation.Leave_time = DateTime.Now;
    var messageString = JsonConvert.SerializeObject(Registation);
    var message = new Message(Encoding.ASCII.GetBytes(messageString));
    Console.WriteLine("Sending message: " + messageString);
    await deviceClient.SendEventAsync(message);
}


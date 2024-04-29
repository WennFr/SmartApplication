using System;
using System.Text;
using Azure.Messaging.EventHubs;
using AzureFunctions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public class SaveDataToCosmosDb
    {
        private readonly ILogger<SaveDataToCosmosDb> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _lampContainer;
        private readonly Container _fanContainer;
        private readonly Container _printerContainer;

        public SaveDataToCosmosDb(ILogger<SaveDataToCosmosDb> logger)
        {
            _logger = logger;

            _cosmosClient = new CosmosClient("AccountEndpoint=https://fw-kyh-cosmosdb.documents.azure.com:443/;AccountKey=15f3YMnW2rduBKyK7z8v1WaNLq8TkjzqooyIyJ3B5eceah4nD9Rh3THHCZKD6lb592fP1BAysUx3ACDbSsOuxQ==;");
            var database = _cosmosClient.GetDatabase("IoTDb");
            _lampContainer = database.GetContainer("lamp_data");
            _fanContainer = database.GetContainer("fan_data");
            _printerContainer = database.GetContainer("printer_data");

        }

        [Function(nameof(SaveDataToCosmosDb))]
        public async Task Run(
            [EventHubTrigger("iothub-ehub-fw-kyh-iot-25230154-a6ffc95e22", ConsumerGroup = "cosmos", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {

                    var json = Encoding.UTF8.GetString(@event.Body.ToArray());

                    dynamic data = null;

                    var container = JsonConvert.DeserializeObject<CosmosContainer>(json);

                    switch (container.ContainerName)
                    {
                        case "lamp_data":
                            data = JsonConvert.DeserializeObject<LampDataMessage>(json)!;
                            await _lampContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;

                        case "fan_data":
                            data = JsonConvert.DeserializeObject<FanDataMessage>(json)!;
                            await _fanContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;
                        case "printer_data":
                            data = JsonConvert.DeserializeObject<PrinterDataMessage>(json)!;
                            await _printerContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;
                        default:
                            _logger.LogWarning($"Unsupported container: {container.ContainerName}");
                            break;
                    }



                    _logger.LogInformation($"Saved Message: {data}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not save: {ex.Message}");
                }
            }
        }
    }
}

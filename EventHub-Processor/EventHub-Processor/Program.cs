using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHub_Processor
{
    class Program
    {
        private static string _bus_connection = "Endpoint=sb://eventhubns204.servicebus.windows.net/;SharedAccessKeyName=hubpolicy;SharedAccessKey=VQqTd4lq05C0BYrZdCdoPywLI2JbSwuHBzumF3QHBmk=;EntityPath=apphub";
        private static string _hubname = "apphub";
        private static string _storage_account = "DefaultEndpointsProtocol=https;AccountName=az204300sa;AccountKey=qkvDp6p5JqN0QjiNwnORHKUk1pMXWnn71F9k8Aj1ftOIL/MX47iH/0WGt2zjP0TbWdLmRz6nq1Yihsuu4ooBxw==;EndpointSuffix=core.windows.net";
        private static string _container = "processor";
        static async Task Main(string[] args)
        {
            BlobContainerClient _blob_client = new BlobContainerClient(_storage_account, _container);

            EventProcessorClient _event_client = new EventProcessorClient(_blob_client, "$Default", _bus_connection, _hubname);

            _event_client.ProcessEventAsync += Process_Message;
            _event_client.ProcessErrorAsync += Error_Handler;

            await _event_client.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(100));

            await _event_client.StopProcessingAsync();

            Console.WriteLine("Completed");

        }
        static async Task Process_Message(ProcessEventArgs eventArgs)
        {
            Console.WriteLine("Getting the events");
            Console.WriteLine(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            // This will update the checkpoint in the storage account
            // This will ensure this handler will only receive newer events
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task Error_Handler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine("An Error has occurred");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}

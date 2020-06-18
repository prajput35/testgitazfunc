using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace queue_send
{
    class Program
    {
        private static string _bus_connectionstring= "Endpoint=sb://servicebusns204.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=sf5kS3w5PQNw2lxAw8boLcmbYUrPr7DK/DXP7NVDtxw=";
        private static string _queue_name = "appqueue";
        static async Task Main(string[] args)
        {
            IQueueClient _client;

            _client = new QueueClient(_bus_connectionstring, _queue_name);

            
            Console.WriteLine("Sending Messages");
            for (int i = 1; i <=10; i++)
            {
                Order obj = new Order();
                
                var _message = new Message(Encoding.UTF8.GetBytes(obj.ToString()));
                _message.TimeToLive = TimeSpan.FromSeconds(30);
                await _client.SendAsync(_message);
                Console.WriteLine($"Sending Message : {obj.Id} ");
            }
        }
        }
}

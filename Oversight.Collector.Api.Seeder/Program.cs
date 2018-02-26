using Newtonsoft.Json;
using Oversight.Collector.Model;
using Splunk.Logging;
using System;
using System.Threading;

namespace Oversight.Collector.Api.Seeder
{
    class Program
    {
        //private static string _collectorApiEndpoint = "https://tnr2n5s4x3.execute-api.us-east-1.amazonaws.com/dev/";
        //private static string _splunkCollectorEndpoint = "https://http-inputs-q4inc.splunkcloud.com";
        
        static void Main(string[] args)
        {
            int[] serverIds = new int[] { 10, 12, 14, 15, 16, 17 };
            Random random = new Random();
            var middleware = new HttpEventCollectorResendMiddleware(3);
            var sender = new HttpEventCollectorSender(
                new Uri("https://http-inputs-q4inc.splunkcloud.com/services/collector/event/"),
                "2550B4EC-5A5F-449F-B6AC-905DCAC57BAD",
                null,
                HttpEventCollectorSender.SendMode.Sequential,
                0,
                0,
                0,
                middleware.Plugin
                );
            sender.OnError += o => Console.WriteLine(o.Message);

            for (var i = 0; i < 10; i++)
            {
                int randServerId = serverIds[random.Next(serverIds.Length)];

                var status = new OversightStatus
                {
                    ServerName = "rc-cache-" + randServerId,
                    ServiceName = "couchbase",
                    Status = "Up",
                    Timestamp = DateTime.Now
                };

                string req = JsonConvert.SerializeObject(status);

                Console.WriteLine("Sending data to splunk");

                try
                {
                    sender.Send(null, null, null, req.Replace("\\", ""));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                Thread.Sleep(30000);
            }

            sender.FlushSync();
        }
    }
}

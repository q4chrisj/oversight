using System;
using System.Net.Http;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using Oversight.Collector.Model;

namespace Oversight.Collector.Api.Seeder
{
    class Program
    {
        private static string _collectorApiEndpoint = "https://tnr2n5s4x3.execute-api.us-east-1.amazonaws.com/dev/";
        private static string _splunkCollectorEndpoint = "https://http-inputs-q4inc.splunkcloud.com:8088";
        
        static void Main(string[] args)
        {
            int[] serverIds = new int[] { 10, 12, 14, 15, 16, 17 };
            Random random = new Random();

            for (var i = 0; i < 1; i++)
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

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_splunkCollectorEndpoint);
                    client.DefaultRequestHeaders.Add("Authorization", "Splunk B5B4C91E-BC17-4A45-9967-B99E16A1A140");

                    HttpResponseMessage response = client.PostAsync("/services/collector", new StringContent(req, Encoding.Default, "application/json")).Result;
                    Console.WriteLine(response.StatusCode);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("There was an error");
                    }
                }
                Thread.Sleep(30000);
            }
        }
    }
}

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
        static void Main(string[] args)
        {
            int[] serverIds = new int[] { 10, 12, 14, 15, 16, 17 };
            Random random = new Random();

            for (var i = 0; i < 100; i++)
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
                    client.BaseAddress = new Uri("https://tnr2n5s4x3.execute-api.us-east-1.amazonaws.com/dev/");

                    HttpResponseMessage response = client.PostAsync("v1/save", new StringContent(req, Encoding.Default, "application/json")).Result;
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

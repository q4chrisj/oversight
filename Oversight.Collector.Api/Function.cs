using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Oversight.Collector
{
    public class Api
    {
        private string _registeredServiceTableName = System.Environment.GetEnvironmentVariable("REGISTEREDSERVICE_TABLE");
        private string _serviceStatusTableName = System.Environment.GetEnvironmentVariable("SERVICESTATUS_TABLE");

        public Api()
        {

        }

        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string>() { { "Context-Type", "text/html" } },
                Body = "Hi"
            };
        }

        public async System.Threading.Tasks.Task<APIGatewayProxyResponse> Save(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            CouchbaseLogRequest data = JsonConvert.DeserializeObject<CouchbaseLogRequest>(request.Body);

            context.Logger.LogLine("Loading table " + _serviceStatusTableName);
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            Table serviceStatus = Table.LoadTable(client, _serviceStatusTableName);
            
            var status = new Document();
            status["id"] = Guid.NewGuid();
            status["ServerName"] = data.ServerName;
            status["ServiceName"] = data.ServiceName;
            status["Status"] = data.Status;
            status["Timestamp"] = data.Timestamp.ToString();

            context.Logger.LogLine("Saving status document");
            try {
                await serviceStatus.PutItemAsync(status);

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)System.Net.HttpStatusCode.OK,
                    Headers = new Dictionary<string, string>() { { "Content-Type", "text/html" } },
                    Body = "The operation completed successfully"
                };
            } catch (AmazonDynamoDBException dex) {
                context.Logger.Log("An error occured: " + dex.Message);

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int) System.Net.HttpStatusCode.InternalServerError,
                    Headers = new Dictionary<string, string>() { { "Content-Type", "text/html"}},
                    Body = dex.Message
                };
            }

            
        }
    }

    public class CouchbaseLogRequest
    {
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp {get; set;}
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Oversight.Collector
{
    public class Api
    {
        public Api()
        {
            LambdaLogger.Log("Starting up the Oversight Collector API\n");
        }

        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string>() { {"Context-Type", "text/html"} },
                Body = "Hi"
            };
        }

        public APIGatewayProxyResponse Save(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            CouchbaseLogRequest data = JsonConvert.DeserializeObject<CouchbaseLogRequest>(request.Body);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ServerName: " + data.ServerName);
            sb.AppendLine("Status: " + data.Status);

            context.Logger.LogLine("ServerName: " + data.ServerName);
            context.Logger.LogLine("Status: " + data.Status);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)System.Net.HttpStatusCode.OK,
                Headers = new Dictionary<string, string>() { {"Content-Type", "text/html"} },
                Body = sb.ToString()
            };
        }
    }

    public class CouchbaseLogRequest 
    {
        public string ServerName {get; set;}
        public string Status {get;set;}
    }
}

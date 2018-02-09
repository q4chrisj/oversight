using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace CollectorApi
{
    public class Collector
    {
        public Collector()
        {

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
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = new Dictionary<string, string>() { {"Context-Type", "text/html"} },
                Body = "Hi"
            };
        }
    }
}

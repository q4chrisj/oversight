using System;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Collector.Api
{
    public class Collector
    {
        public Collector()
        {

        }

        public APIGatewayProxyResponse GetA(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = null,
                Body = "Hi"
            };
        }

        public APIGatewayProxyResponse Save(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Headers = null,
                Body = "Hi"
            };
        }
    }
}

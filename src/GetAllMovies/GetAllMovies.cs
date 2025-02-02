using GetAllMovies.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GetAllMovies
{
    public class GetAllMovies
    {
        private readonly ILogger<GetAllMovies> _logger;
        private readonly CosmosClient _cosmosClient;

        public GetAllMovies(ILogger<GetAllMovies> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("details")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var container = _cosmosClient.GetContainer("Movies", "Items");
            var query = new QueryDefinition("SELECT * FROM Movies WHERE m");
            var result = container.GetItemQueryIterator<MovieResponse>(query);
            var results = new List<MovieResponse>();
            while (result.HasMoreResults)
            {
                foreach (var item in await result.ReadNextAsync())
                {
                    results.Add(item);
                }
            }

            var responseMessage = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await responseMessage.WriteAsJsonAsync(results);

            return responseMessage;
        }
    }
}

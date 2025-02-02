using GetMovieDetail.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GetMovieDetail
{
    public class GetMovieDetail
    {
        private readonly ILogger<GetMovieDetail> _logger;
        private readonly CosmosClient _cosmosClient;

        public GetMovieDetail(ILogger<GetMovieDetail> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("detail")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var container = _cosmosClient.GetContainer("Movies", "Items");
            var id = req.Query["id"];
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", id);
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
            await responseMessage.WriteAsJsonAsync(results.FirstOrDefault());

            return responseMessage;
        }
    }
}

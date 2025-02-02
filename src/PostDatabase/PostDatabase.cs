using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PostDatabase.Models.Requests;

namespace PostDatabase
{
    public class PostDatabase
    {
        private readonly ILogger<PostDatabase> _logger;

        public PostDatabase(ILogger<PostDatabase> logger)
        {
            _logger = logger;
        }

        [Function("movie")]
        [CosmosDBOutput("%DatabaseName%", "Movies", Connection = "CosmosDbConnection", CreateIfNotExists = true, PartitionKey = "Id")]
        public async Task<object?> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var movie = JsonConvert.DeserializeObject<MovieRequest>(content);
                return JsonConvert.SerializeObject(movie);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Erro ao deserializar o objeto: " + ex.Message);
            }
        }
    }
}

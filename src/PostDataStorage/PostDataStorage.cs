using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DataStorage
{
    public class PostDataStorage
    {
        private readonly ILogger<PostDataStorage> _logger;

        public PostDataStorage(ILogger<PostDataStorage> logger)
        {
            _logger = logger;
        }

        [Function("data-storage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processando a imagem no Storage");
            try
            {
                if(!req.Headers.TryGetValue("file-type", out var fileTypeHeader))
                {
                    return new BadRequestObjectResult("O cabeçalho 'file-type' é obrigatório");
                }

                var fileType = fileTypeHeader.ToString();
                var form = await req.ReadFormAsync();
                var file = form.Files["file"];

                if(file is null || file.Length == 0)
                {
                    return new BadRequestObjectResult("O arquivo não foi enviado");
                }

                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? string.Empty;
                string containerName = fileType;

                BlobClient blobClient = new BlobClient(connectionString, containerName, file.FileName);
                BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

                await blobContainerClient.CreateIfNotExistsAsync();
                await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
                await blobClient.UploadAsync(file.OpenReadStream(), true);

                string blobName = file.FileName;
                var blob = blobContainerClient.GetBlobClient(blobName);

                using(var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream, true);
                }

                _logger.LogInformation($"Arquivo {file.FileName} armazenado com sucesso!");

                return new OkObjectResult(new
                {
                    Message = $"Arquivo {file.FileName} armazenado com sucesso!",
                    BlobUrl = blob.Uri
                });
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}

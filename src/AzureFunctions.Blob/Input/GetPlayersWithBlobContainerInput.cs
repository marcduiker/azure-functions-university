using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;
using AzureFunctionsUniversity.Demo.Blob.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Azure.Storage.Blobs;
using System;

namespace AzureFunctionsUniversity.Demo.Blob.Input
{
    public static class GetPlayersWithBlobContainerInput
    {
        
        // TODO
        // [FunctionName(nameof(GetPlayersWithBlobContainerInput))]
        // public static async Task<IActionResult> Run(
        //     [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request,
        //     [Blob("players", FileAccess.Read)] CloudBlobContainer cloudBlobContainer
        // )
        // {
            
        //     var blobList = cloudBlobContainer.ListBlobs(
        //         prefix: "in/");
        //     var connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        //     var client = new BlobContainerClient(connection, "players");
        //     var blobs = client.GetBlobs(prefix: "in/");
            
        //     return new OkObjectResult(blobList.First().Container.Name);
        // }
    }
}

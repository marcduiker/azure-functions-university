using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AzureFunctionsUniversity.Demo.Blob.Input
{
    public static class GetBlobNamesWithContainerBlobInput
    {
        [FunctionName(nameof(GetBlobNamesWithContainerBlobInput))]
         public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                nameof(HttpMethods.Get),
                Route = null)] HttpRequest request,
            [Blob(
                "players",
                FileAccess.Read)] CloudBlobContainer cloudBlobContainer)
        {
            var blobList = cloudBlobContainer
                .ListBlobs(prefix: "in/")
                .OfType<CloudBlockBlob>();
            var blobNames = blobList.Select(blob => new { BlobName = blob.Name });

            return new OkObjectResult(blobNames);
        }
    }
}

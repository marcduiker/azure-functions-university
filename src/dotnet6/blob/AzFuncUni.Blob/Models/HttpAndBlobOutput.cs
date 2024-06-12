using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzFuncUni.Blob.Models
{
	public class HttpAndBlobOutput
    {
        public HttpAndBlobOutput(
            string blobData,
            HttpResponseData httpData)
        {
            BlobData = blobData;
            HttpData = httpData;
        }
        
        [BlobOutput("players/out/string-{rand-guid}.json", Connection = "AzureWebJobsBlobStorage")]
        public string BlobData { get; set; }

        public HttpResponseData HttpData { get; set; }
    }
}
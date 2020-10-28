using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MadeByGPS.Function
{
    public static class Resume
    {
        [FunctionName("Resume")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            ResumeData resumeData = new ResumeData
            {

                firstName = "Gwyneth",
                lastName = "Pena-Siguenza",
                location = "CT, USA",
                currentPosition = "Cloud Consultant and YouTuber",

            };

            string json = JsonConvert.SerializeObject(resumeData, Formatting.Indented);


            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {

                Content = new StringContent(json, Encoding.UTF8, "application/json")

            };
        }
    }

    public class ResumeData    {
        public string firstName { get;set;}
        public string lastName { get; set; } 
        public string location { get; set; } 
        public string currentPosition { get; set; } 
    
    }

}

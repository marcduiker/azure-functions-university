using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Durable.Chaining.GitHub
{
    public static class GitHubInfoOrchestrator
    {
        [FunctionName("GitHubInfoOrchestrator")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<string>();

            var userName = await context.CallActivityAsync<string>("GitHubInfoOrchestrator_GetRepositoryDetailsByName", input);

            var userDetails = await context.CallActivityAsync<string>("GitHubInfoOrchestrator_GetUserDetailsByName", userName);

            return userDetails;
        }

        [FunctionName("GitHubInfoOrchestrator_GetRepositoryDetailsByName")]
        public static async Task<string> GetRepositoryDetails([ActivityTrigger] string name, ILogger log)
        {
            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var request = new SearchRepositoriesRequest(name);

            var searchResult = await githubClient.Search.SearchRepo(request);
            var repository = searchResult.Items.Single(x => x.Name == name);

            return repository.Owner.Login;
        }

        [FunctionName("GitHubInfoOrchestrator_GetUserDetailsByName")]
        public static async Task<string> GetUserDetailsByName([ActivityTrigger] string name, ILogger log)
        {
            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var user = await githubClient.User.Get(name);

            var serializedUser = JsonSerializer.Serialize<object>(user);
            return serializedUser;
        }

        [FunctionName("GitHubInfoOrchestrator_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string name = req.Query["name"];

            string instanceId = await starter.StartNewAsync<string>("GitHubInfoOrchestrator", name);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}

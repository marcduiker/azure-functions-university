using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Durable.Chaining.GitHub.Timeout
{
    public static class GitHubInfoTimeoutOrchestrator
    {
        [FunctionName("GitHubInfoTimeoutOrchestrator")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var input = context.GetInput<object>();

            using var cancellationTokenSource = new CancellationTokenSource();

            DateTime dueTime = context.CurrentUtcDateTime.AddSeconds(3);
            var timerTask = context.CreateTimer(dueTime, cancellationTokenSource.Token);

            var repositoryTaskDetail = context.CallActivityAsync<string>(
                "GitHubInfoTimeoutOrchestrator_GetRepositoryDetailsByName",
                input);

            var winner = await Task.WhenAny(repositoryTaskDetail, timerTask);

            if (winner == repositoryTaskDetail)
            {
                log.LogInformation("Repository information fetched before timeout");

                cancellationTokenSource.Cancel(false);
            }
            else
            {
                log.LogWarning("Repository information call timed out...");
                throw new Exception("Repository information call timed out");
            }

            var userName = repositoryTaskDetail.Result;
            var userDetails = await context.CallActivityAsync<string>("GitHubInfoTimeoutOrchestrator_GetUserDetailsByName", userName);

            return userDetails;
        }

        [FunctionName("GitHubInfoTimeoutOrchestrator_GetRepositoryDetailsByName")]
        public static async Task<string> GetRepositoryDetails([ActivityTrigger] string name, ILogger log)
        {
            await Task.Delay(10000);
            
            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var request = new SearchRepositoriesRequest(name);

            var searchResult = await githubClient.Search.SearchRepo(request);
            var repository = searchResult.Items.Single(x => x.Name == name);

            return repository.Owner.Login;
        }

        [FunctionName("GitHubInfoTimeoutOrchestrator_GetUserDetailsByName")]
        public static async Task<string> GetUserDetailsByName([ActivityTrigger] string name, ILogger log)
        {
            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var user = await githubClient.User.Get(name);

            var serializedUser = JsonSerializer.Serialize<object>(user);
            return serializedUser;
        }

        [FunctionName("GitHubInfoTimeoutOrchestrator_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string name = req.Query["name"];

            string instanceId = await starter.StartNewAsync<string>("GitHubInfoTimeoutOrchestrator", name);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}

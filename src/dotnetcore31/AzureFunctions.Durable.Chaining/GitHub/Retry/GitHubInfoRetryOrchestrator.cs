using System;
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

namespace Durable.Chaining.GitHub.Retry
{
    public static class GitHubInfoRetryOrchestrator
    {
        private const int FirstRetryIntervalInMilliseconds = 1000;
        private const int MaxNumberOfAttempts = 3;
        private const int MaxRetryIntervalInMilliseconds = 1000;
        private const int RetryTimeoutInMilliseconds = 7000;

        [FunctionName("GitHubInfoRetryOrchestrator")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<object>();

            var retryOptions = new RetryOptions(
                firstRetryInterval: TimeSpan.FromMilliseconds(FirstRetryIntervalInMilliseconds),
                maxNumberOfAttempts: MaxNumberOfAttempts)
            {
                MaxRetryInterval = TimeSpan.FromMilliseconds(MaxRetryIntervalInMilliseconds),
                RetryTimeout = TimeSpan.FromMilliseconds(RetryTimeoutInMilliseconds)
            };
            
            var raiseException = !context.IsReplaying;

            var activityParams = new string[]
            {
                input.ToString(), raiseException.ToString()
            };

            var userName = await context.CallActivityWithRetryAsync<string>(
                "GitHubInfoRetryOrchestrator_GetRepositoryDetailsByName",
                retryOptions,
                activityParams);

            var userDetails = await context.CallActivityWithRetryAsync<string>(
                "GitHubInfoRetryOrchestrator_GetUserDetailsByName",
                retryOptions,
                userName);

            return userDetails;
        }

        [FunctionName("GitHubInfoRetryOrchestrator_GetRepositoryDetailsByName")]
        public static async Task<string> GetRepositoryDetails([ActivityTrigger] string[] inputs, ILogger log)
        {
            var name = inputs[0];
            var raiseException = bool.Parse(inputs[1]);

            if (raiseException)
                throw new Exception("Stop!");

            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var request = new SearchRepositoriesRequest(name);

            var searchResult = await githubClient.Search.SearchRepo(request);
            var repository = searchResult.Items.Single(x => x.Name == name);

            return repository.Owner.Login;
        }

        [FunctionName("GitHubInfoRetryOrchestrator_GetUserDetailsByName")]
        public static async Task<string> GetUserDetailsByName([ActivityTrigger] string name, ILogger log)
        {
            var githubClient = new GitHubClient(new ProductHeaderValue("azure-functions-university"));

            var user = await githubClient.User.Get(name);

            var serializedUser = JsonSerializer.Serialize<object>(user);
            return serializedUser;
        }

        [FunctionName("GitHubInfoRetryOrchestrator_HttpStart")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string name = req.Query["name"];

            string instanceId = await starter.StartNewAsync<string>("GitHubInfoRetryOrchestrator", name);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}

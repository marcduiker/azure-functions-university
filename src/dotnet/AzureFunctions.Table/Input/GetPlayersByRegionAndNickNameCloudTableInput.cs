using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctionsUniversity.Models;

namespace AzureFunctionsUniversity.Table.Input
{
    public static class GetPlayersByRegionAndNickNameCloudTableInput
    {
        [FunctionName(nameof(GetPlayersByRegionAndNickNameCloudTableInput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = null)] HttpRequest request,
            [Table(TableConfig.Table)] CloudTable cloudTable)
        {
            string region = request.Query["region"];
            string nickName = request.Query["nickName"];

            var regionAndNickNameFilter = new TableQuery<PlayerEntity>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition(
                            nameof(PlayerEntity.PartitionKey), 
                            QueryComparisons.Equal,
                            region),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition(
                            nameof(PlayerEntity.NickName),
                            QueryComparisons.Equal,
                            nickName)));
            var playerEntities = cloudTable.ExecuteQuery<PlayerEntity>(regionAndNickNameFilter);

            return new OkObjectResult(playerEntities);
        }
    }
}

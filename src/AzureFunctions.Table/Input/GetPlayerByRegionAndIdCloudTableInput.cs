using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctionsUniversity.Models;

namespace AzureFunctionsUniversity.Table.Input
{
    public static class GetPlayerByRegionAndIdTableInput
    {
        [FunctionName(nameof(GetPlayerByRegionAndIdTableInput))]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Get),
                Route = "GetPlayerByRegionAndIdTableInput/{region}/{id}")] HttpRequest request,
            string region,
            string id,
            [Table(
                TableConfig.Table,
                "{region}",
                "{id}")] PlayerEntity playerEntity)
        {
            return new OkObjectResult(playerEntity);
        }
    }
}

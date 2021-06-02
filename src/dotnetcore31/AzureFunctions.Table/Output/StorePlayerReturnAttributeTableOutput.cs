using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctionsUniversity.Models;

namespace AzureFunctionsUniversity.Table.Output
{
    public static class StorePlayerReturnAttributeTableOutput
    {
        [FunctionName(nameof(StorePlayerReturnAttributeTableOutput))]
        [return: Table(TableConfig.Table)]
        public static PlayerEntity Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                nameof(HttpMethods.Post),
                Route = null)] PlayerEntity playerEntity)
        {
            playerEntity.SetKeys();

            return playerEntity;
        }
    }
}

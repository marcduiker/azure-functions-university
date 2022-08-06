using AzureFunctionsUniversity.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

public class PlayersRepository
{

    private readonly Container _playersCollection;


    public PlayersRepository(CosmosClient client, IConfiguration configuration)
    {
        var database = client.GetDatabase(configuration["CosmosDBConnection"]);
        _playersCollection = database.GetContainer("Players");
       
    }

    public async Task AddPlayerAsync(Player player)
    {
        player.Id = Guid.NewGuid().ToString("N");
        await _playersCollection.UpsertItemAsync(player, new PartitionKey(player.Id));
    }

}
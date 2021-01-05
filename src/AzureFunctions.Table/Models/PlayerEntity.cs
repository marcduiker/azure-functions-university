using Microsoft.Azure.Cosmos.Table;

namespace AzureFunctionsUniversity.Models
{
    public class PlayerEntity : TableEntity
    {
        public PlayerEntity()
        {}
        public PlayerEntity(
            string region,
            string id,
            string nickName,
            string email) 
            : base(partitionKey: region, rowKey: id)
        {
            Region = region;
            Id = id;
            NickName = nickName;
            Email = email; 
        }

        public string Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }

        public void SetKeys()
        {
            PartitionKey = Region;
            RowKey = Id;
        }
    }
}
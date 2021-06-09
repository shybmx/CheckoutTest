using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class CosmosDatabaseWrapper<T> : ICosmosDatabaseWrapper<T> where T : class
    {
        private CosmosClient _cosmosClient;
        private Database _cosmosDatabase;
        private Container _cosmosContainer;

        public CosmosDatabaseWrapper(CosmosConfiguration cosmosConfig)
        {
            _cosmosClient = new CosmosClient(cosmosConfig.EndpointUri, cosmosConfig.PrimaryKey);
            _cosmosDatabase = _cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosConfig.DatabaseName).Result;
            _cosmosContainer = _cosmosDatabase.CreateContainerIfNotExistsAsync(cosmosConfig.ContainerName, cosmosConfig.PartitonKey).Result;
        }

        public async Task CreateItemAsync<T>(T itemToSend, string partitionKey)
        {
            await _cosmosContainer.CreateItemAsync<T>(itemToSend, new PartitionKey(partitionKey));
        }

        public FeedIterator<T> GetItemAsync<T>(QueryDefinition queryDefinition)
        {
            return _cosmosContainer.GetItemQueryIterator<T>(queryDefinition);
        }
    }
}

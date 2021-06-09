using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.Azure.Cosmos;
using System;
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

        public async Task<T> GetItemAsync<T>(Guid indentifer)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{indentifer}'";

            var queryDefinition = new QueryDefinition(sqlQueryText);

            var queryResultSetIterator = _cosmosContainer.GetItemQueryIterator<T>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                var items = await queryResultSetIterator.ReadNextAsync();
                foreach (var item in items)
                {
                    return item;
                }
            }

            return default;
        }
    }
}

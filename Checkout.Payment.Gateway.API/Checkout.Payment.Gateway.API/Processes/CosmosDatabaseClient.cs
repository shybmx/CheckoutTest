using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.Azure.Cosmos;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class CosmosDatabaseClient : ICosmosDatabaseClient 
    {
        private CosmosClient _cosmosClient;
        private Database _cosmosDatabase;
        private Container _cosmosContainer; 

        public CosmosDatabaseClient(CosmosConfiguration cosmosConfig)
        {
            _cosmosClient = new CosmosClient(cosmosConfig.EndpointUri, cosmosConfig.PrimaryKey);
            _cosmosDatabase = _cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosConfig.DatabaseName).Result;
            _cosmosContainer = _cosmosDatabase.CreateContainerIfNotExistsAsync(cosmosConfig.ContainerName, cosmosConfig.PartitonKey).Result;
        }

       
    }
}

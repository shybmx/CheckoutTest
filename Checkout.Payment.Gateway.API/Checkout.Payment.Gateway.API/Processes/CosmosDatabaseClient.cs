using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

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

        public async Task<SavedPaymentDetails> GetPaymentDetails(Guid indentifer)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{indentifer}'";

            try
            {
                var queryDefinition = new QueryDefinition(sqlQueryText);
                var queryResultSetIterator = _cosmosContainer.GetItemQueryIterator<SavedPaymentDetails>(queryDefinition);
                while (queryResultSetIterator.HasMoreResults)
                {
                    var items = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in items)
                    {
                        return item;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return null;
        }

        public async Task SavePaymentDetails(PaymentDetails paymentDetails, BankResponse bankResponse)
        {
            var savedPaymentDetails = new SavedPaymentDetails
            {
                Amount = paymentDetails.Amount,
                CardNumber = paymentDetails.CardNumber,
                Currency = paymentDetails.Currency,
                Cvv = paymentDetails.Cvv,
                Expiry = paymentDetails.Expiry,
                Identifier = bankResponse.Identifier,
                IsSuccessful = bankResponse.PaymentSuccessful
            };

            try
            {
                await _cosmosContainer.CreateItemAsync<SavedPaymentDetails>(savedPaymentDetails, new PartitionKey(savedPaymentDetails.Currency));
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

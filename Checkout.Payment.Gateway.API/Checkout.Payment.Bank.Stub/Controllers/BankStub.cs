using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Bank.Stub.Controllers
{
    [ApiController]
    [Route("Banking")]
    public class BankStub : ControllerBase
    {
        private static readonly string EndpointUri = "https://checkoutshaytest.documents.azure.com:443/";
        private static readonly string PrimaryKey = "zyjUyKuyxHtrWz3YIyBM9Ba02txAJikbatlxUUZNGqGGwUJURZKwzUu3Lj8VtjEKmAGSfkS8myG7P9wFCPRVXg==";
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        [HttpGet("executePurchase")]
        public async Task<IActionResult> BankExecutePurchase([FromQuery] PaymentDetails paymentDetails)
        {
            await CreateCosmosConnection();

            try
            {
                paymentDetails.Identifier = Guid.NewGuid();
                var paymentToUpload = await _container.CreateItemAsync<PaymentDetails>(paymentDetails, new PartitionKey(paymentDetails.Currency));
                return new OkObjectResult(new BankResponse { Identifier = paymentDetails.Identifier, PaymentSuccessful = true });
            }
            catch(Exception e)
            {
                return new OkObjectResult(new BankResponse { PaymentSuccessful = false });
            }
        }

        [HttpGet("getDetails")]
        public async Task<IActionResult> GetPurchase([FromQuery] Guid paymentGuid)
        {
            await CreateCosmosConnection();

            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{paymentGuid}'";

            try
            {
                var queryDefinition = new QueryDefinition(sqlQueryText);
                var queryResultSetIterator = _container.GetItemQueryIterator<PaymentDetails>(queryDefinition);
                while (queryResultSetIterator.HasMoreResults)
                {
                    var items = await queryResultSetIterator.ReadNextAsync();
                    foreach (var item in items)
                    {
                        return new OkObjectResult(item);
                    }
                }
            }
            catch(Exception e)
            {
                return new NotFoundObjectResult("Payment Not Found");
            }

            return new NotFoundObjectResult("Payment Not Found");
        }

        private async Task CreateCosmosConnection()
        {
            _cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("bank");
            _container = await _database.CreateContainerIfNotExistsAsync("bankConatiner", "/Currency");
        }
    }
}

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

        [HttpGet]
        public async Task<IActionResult> BankExecutePurchase([FromQuery] PaymentDetails paymentDetails)
        {
            _cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("bank");
            _container = await _database.CreateContainerAsync("bankConatiner", "/partition");

            try
            {
                var paymentToUpload = await _container.CreateItemAsync<PaymentDetails>(paymentDetails, new PartitionKey(paymentDetails.Currency));
                return new OkObjectResult(new BankResponse { Identifier = new Guid(), PaymentSuccessful = true });
            }
            catch(Exception e)
            {
                return new OkObjectResult(new BankResponse { PaymentSuccessful = false });
            }
        }
    }
}

using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Interfaces
{
    public interface ICosmosDatabaseWrapper<T> where T : class
    {
        Task CreateItemAsync<T>(T itemToSend, string partitonKey);
        Task<T> GetItemAsync<T>(Guid identifier);
    }
}

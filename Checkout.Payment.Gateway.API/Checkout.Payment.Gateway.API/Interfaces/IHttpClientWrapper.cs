using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}

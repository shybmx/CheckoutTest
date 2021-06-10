using Checkout.Payment.Gateway.API.Interfaces;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient _client;
        
        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _client.Send(request);
        }
    }
}

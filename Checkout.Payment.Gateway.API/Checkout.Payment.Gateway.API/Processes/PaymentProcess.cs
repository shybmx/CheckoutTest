using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class PaymentProcess : IPaymentProcess
    {
        private HttpClient _httpClient;

        public PaymentProcess(PaymentConfiguration paymentConfig)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(paymentConfig.Endpoint);
        }

        public async Task<BankResponse> SendPayment(PaymentDetails paymentDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentDetails), Encoding.UTF8,
                    "application/json")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return new BankResponse();
        }
    }
}

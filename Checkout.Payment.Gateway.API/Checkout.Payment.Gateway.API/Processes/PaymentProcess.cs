using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class PaymentProcess : IPaymentProcess
    {
        private IHttpClientWrapper _httpClientWrapper;
        private PaymentConfiguration _paymentConfig;

        public PaymentProcess(PaymentConfiguration paymentConfig)
        {
            _paymentConfig = paymentConfig;
        }

        public async Task<BankResponse> SendPayment(PaymentDetails paymentDetails)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _paymentConfig.Endpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(paymentDetails), Encoding.UTF8,
                    "application/json")
            };

            var response = await _httpClientWrapper.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var bankResponse = JsonConvert.DeserializeObject<BankResponse>(await response.Content.ReadAsStringAsync());

            return bankResponse;
        }
    }
}

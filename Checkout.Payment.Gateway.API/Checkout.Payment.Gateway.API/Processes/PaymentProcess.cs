using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using System;
using System.Net.Http;
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
             await _httpClient.GetAsync<BankResponse>(new HttpRequestMessage());
        }
    }
}

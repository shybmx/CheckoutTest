using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using System;
using System.Net.Http;

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
    }
}

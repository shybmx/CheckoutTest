using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class PaymentProcess : IPaymentProcess
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private PaymentConfiguration _paymentConfig;
        private AsyncRetryPolicy _retryPolicy;

        public PaymentProcess(PaymentConfiguration paymentConfig, IHttpClientWrapper httpClientWrapper)
        {
            _paymentConfig = paymentConfig;
            _httpClientWrapper = httpClientWrapper;
            _retryPolicy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(new[] { TimeSpan.FromMilliseconds(500) });
        }

        public async Task<BankResponse> SendPayment(PaymentDetails paymentDetails)
        {
            var request = BuildHttpRequest("/Banking/executePurchase");

            request.Content = new StringContent(JsonConvert.SerializeObject(paymentDetails), Encoding.UTF8,
                    "application/json");

            try
            {
                var response = await _retryPolicy.ExecuteAsync(() => _httpClientWrapper.SendAsync(request));

                if (response == null || !response.IsSuccessStatusCode)
                {
                    return null;
                }

                var bankResponse = JsonConvert.DeserializeObject<BankResponse>(await response.Content.ReadAsStringAsync());

                return bankResponse;

            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentDetails> GetPaymentDetails(Guid identifer)
        {
            var request = BuildHttpRequest("/GetPurchaseDetails");

            request.Content = new StringContent(JsonConvert.SerializeObject(identifer), Encoding.UTF8,
                    "application/json");

            try
            {
                var response = await _retryPolicy.ExecuteAsync(() => _httpClientWrapper.SendAsync(request));

                if (response == null || !response.IsSuccessStatusCode)
                {
                    return null;
                }

                var paymentDetailsResponse = JsonConvert.DeserializeObject<PaymentDetails>(await response.Content.ReadAsStringAsync());

                return paymentDetailsResponse;

            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private HttpRequestMessage BuildHttpRequest(string endpoint)
        {
            return new HttpRequestMessage(HttpMethod.Post, _paymentConfig.Endpoint + endpoint);
        }
    }
}

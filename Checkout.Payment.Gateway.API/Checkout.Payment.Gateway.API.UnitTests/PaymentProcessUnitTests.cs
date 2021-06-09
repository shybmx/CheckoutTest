using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.API.Processes;
using Checkout.Payment.Gateway.Contracts;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class PaymentProcessUnitTests
    {
        private IPaymentProcess _paymentProcess;
        private Mock<IHttpClientWrapper> _httpClientWrapperMock;
        private PaymentDetails _paymentDetails;
        private BankResponse _bankResponse;

        [SetUp]
        public void Setup()
        {
            var paymentConfig = new PaymentConfiguration {
                Endpoint = "https://www.test.com/"
            };

            _bankResponse = new BankResponse()
            {
                Identifier = Guid.NewGuid(),
                PaymentSuccessful = true
            };

            _paymentDetails = new PaymentDetails
            {
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Expiry = TestConstant.Expiry,
                Amount = TestConstant.Amount
            };

            _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
   
            _paymentProcess = new PaymentProcess(paymentConfig, _httpClientWrapperMock.Object);
        }

        [Test] 
        public async Task Given_Valid_Payment_Process_Should_Return_Correct_Object()
        {
            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
                new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(_bankResponse))});

            var actual = await _paymentProcess.SendPayment(_paymentDetails);

            Assert.IsNotNull(actual);
            Assert.That(actual.PaymentSuccessful, Is.EqualTo(_bankResponse.PaymentSuccessful));
        }

        [Test]
        public async Task Given_Invalid_Payment_Process_Should_Return_Null()
        {
            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
                new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var actual = await _paymentProcess.SendPayment(null);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task Given_Valid_Payment_Process_But_API_Call_Fails_Should_Throw_Exception()
        {
            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).Throws(new Exception());

            Assert.ThrowsAsync<Exception>(() => _paymentProcess.SendPayment(_paymentDetails));
        }

        [Test]
        public async Task Given_Valid_Payment_Process_But_Json_Cannot_Deserialize_Should_Throw_Json_Exception()
        {
            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
                new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("test") });

            Assert.ThrowsAsync<Exception>(() => _paymentProcess.SendPayment(_paymentDetails));
        }

        [Test]
        public async Task Given_Valid_Guid_Should_Return_Payment_Details()
        {
            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
            new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(_paymentDetails)) });

            var actual = await _paymentProcess.GetPaymentDetails(Guid.NewGuid());

            Assert.IsNotNull(actual);
        }
        
    }
}

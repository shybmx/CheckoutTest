﻿using Checkout.Payment.Gateway.API.Controllers;
using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.API.Processes;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class PaymentProcessUnitTests
    {
        private IPaymentProcess _paymentProcess;
        private Mock<IHttpClientWrapper> _httpClientWrapperMock;
        private PaymentDetails _paymentDetails;

        [SetUp]
        public void Setup()
        {
            var paymentConfig = new PaymentConfiguration {
                Endpoint = "https://www.test.com/"
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
            var actual = await _paymentProcess.SendPayment(_paymentDetails);

            _httpClientWrapperMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("4")});

            Assert.IsNotNull(actual);
        }

        
    }
}

using Checkout.Payment.Gateway.API.Controllers;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class PaymentGatewayUnitTests
    {
        private PaymentGateway _paymentGatewayController;

        [SetUp]
        public void Setup()
        {
            _paymentGatewayController = new PaymentGateway();
            
        }

        [Test]
        public async Task Given_Payment_Details_Is_Null_Should_Response_With_Bad_Response()
        {
            var actual = await _paymentGatewayController.ProcessPayment(null);

            Assert.That(actual.GetType, Is.EqualTo(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task Given_Payment_Details_Is_Not_Null_Should_Response_With_SuccessfulAsync()
        {
            var paymentDetails = new PaymentDetails { 
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry
            };

            var actual = await _paymentGatewayController.ProcessPayment(paymentDetails);

            Assert.That(actual.GetType, Is.EqualTo(typeof(OkObjectResult)));
        }
        
    }
}
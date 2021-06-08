using Checkout.Payment.Gateway.API.Controllers;
using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class PaymentGatewayUnitTests
    {
        private PaymentGateway _paymentGatewayController;
        private Mock<IPaymentProcess> _paymentProcessMock;

        [SetUp]
        public void Setup()
        {
            _paymentProcessMock = new Mock<IPaymentProcess>();

            _paymentGatewayController = new PaymentGateway(_paymentProcessMock.Object);
        }

        [Test]
        public async Task Given_Payment_Details_Is_Null_Should_Response_With_Bad_Response()
        {
            var actual = await _paymentGatewayController.ProcessPayment(null);

            Assert.That(actual.GetType, Is.EqualTo(typeof(BadRequestObjectResult)));
        }

        [Test]
        public async Task Given_Payment_Details_Is_Not_Null_And_Payment_Successful_Should_Response_With_Successful()
        {
            var paymentDetails = new PaymentDetails { 
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry
            };

            _paymentProcessMock.Setup(x => x.SendPayment(It.IsAny<PaymentDetails>())).ReturnsAsync(new BankResponse());

            var actual = await _paymentGatewayController.ProcessPayment(paymentDetails);

            Assert.That(actual.GetType, Is.EqualTo(typeof(OkObjectResult)));
        }

        [Test]
        public async Task Given_Payment_Details_Is_Not_Null_And_Payment_Not_Successful_Should_Response_With_Successful()
        {
            var paymentDetails = new PaymentDetails
            {
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry
            };

            _paymentProcessMock.Setup(x => x.SendPayment(It.IsAny<PaymentDetails>()));

            var actual = await _paymentGatewayController.ProcessPayment(paymentDetails);

            Assert.That(actual.GetType, Is.EqualTo(typeof(NotFoundObjectResult)));
        }
    }
}
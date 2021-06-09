using Checkout.Payment.Gateway.API.Controllers;
using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class PaymentGatewayUnitTests
    {
        private PaymentGateway _paymentGatewayController;
        private PaymentDetails _paymentDetails;
        private Mock<IPaymentProcess> _paymentProcessMock;
        private Mock<ICosmosDatabaseClient> _cosmosDatabaseClientMock;

        [SetUp]
        public void Setup()
        {
            _paymentProcessMock = new Mock<IPaymentProcess>();
            _cosmosDatabaseClientMock = new Mock<ICosmosDatabaseClient>();

            _paymentDetails = new PaymentDetails
            {
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry
            };

            _cosmosDatabaseClientMock.Setup(x => x.SavePaymentDetails(It.IsAny<PaymentDetails>(), It.IsAny<BankResponse>()));

            _paymentGatewayController = new PaymentGateway(_paymentProcessMock.Object, _cosmosDatabaseClientMock.Object);
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
            _paymentProcessMock.Setup(x => x.SendPayment(It.IsAny<PaymentDetails>())).ReturnsAsync(new BankResponse());

            var actual = await _paymentGatewayController.ProcessPayment(_paymentDetails);

            Assert.That(actual.GetType, Is.EqualTo(typeof(OkObjectResult)));
        }

        [Test]
        public async Task Given_Payment_Details_Is_Not_Null_And_Payment_Not_Successful_Should_Response_With_NotFound()
        {
            _paymentProcessMock.Setup(x => x.SendPayment(It.IsAny<PaymentDetails>()));

            var actual = await _paymentGatewayController.ProcessPayment(_paymentDetails);

            Assert.That(actual.GetType, Is.EqualTo(typeof(NotFoundObjectResult)));
        }

        [Test]
        public async Task Given_Identifer_And_Found_In_Database_Should_Respone_With_Success()
        {
            _cosmosDatabaseClientMock.Setup(x => x.GetPaymentDetails(It.IsAny<Guid>())).ReturnsAsync(new PaymentDetails());

            var actual = await _paymentGatewayController.GetPaymentDetails(Guid.NewGuid());

            Assert.That(actual.GetType, Is.EqualTo(typeof(OkObjectResult)));
        }

        [Test]
        public async Task Given_Identifer_And_Not_Found_In_Database_Should_Respone_With_NotFound()
        {
            _cosmosDatabaseClientMock.Setup(x => x.GetPaymentDetails(It.IsAny<Guid>()));

            var actual = await _paymentGatewayController.GetPaymentDetails(Guid.NewGuid());

            Assert.That(actual.GetType, Is.EqualTo(typeof(NotFoundObjectResult)));
        }
    }
}
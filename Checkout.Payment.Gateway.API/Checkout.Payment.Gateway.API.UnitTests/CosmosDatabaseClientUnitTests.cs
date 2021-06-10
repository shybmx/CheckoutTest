using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.API.Processes;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public class CosmosDatabaseClientUnitTests
    {
        private ICosmosDatabaseClient _cosmosDatabaseClient;
        private Mock<ICosmosDatabaseWrapper<SavedPaymentDetails>> _cosmosWrapperMock;
        private PaymentDetails _paymentDetails;
        private BankResponse _BankResponse;
        private SavedPaymentDetails _savedPaymentDetails;

        [SetUp]
        public void Setup()
        {
            _cosmosWrapperMock = new Mock<ICosmosDatabaseWrapper<SavedPaymentDetails>>();

            _savedPaymentDetails = new SavedPaymentDetails
            {
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry,
                IsSuccessful = true
            };

            _paymentDetails = new PaymentDetails
            {
                CardNumber = TestConstant.CardNumber,
                Currency = TestConstant.Currency,
                Cvv = TestConstant.Cvv,
                Amount = TestConstant.Amount,
                Expiry = TestConstant.Expiry
            };

            _BankResponse = new BankResponse()
            {
                PaymentSuccessful = true
            };

            _cosmosDatabaseClient = new CosmosDatabaseClient(_cosmosWrapperMock.Object);
        }

        [Test]
        public async Task Given_Payment_Details_And_Bank_Responses_Save_Payment_Details_Should_Insert_Into_Cosmos()
        {
            await _cosmosDatabaseClient.SavePaymentDetails(_paymentDetails, _BankResponse);

            _cosmosWrapperMock.Verify(x => x.CreateItemAsync<SavedPaymentDetails>(It.IsAny<SavedPaymentDetails>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Given_Payment_Details_Is_Null_And_Bank_Responses_Save_Payment_Details_Should_Not_Insert_Into_Cosmos()
        {
            await _cosmosDatabaseClient.SavePaymentDetails(null, _BankResponse);

            _cosmosWrapperMock.Verify(x => x.CreateItemAsync<SavedPaymentDetails>(It.IsAny<SavedPaymentDetails>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Given_Payment_Details_And_Bank_Responses_Is_Null_Save_Payment_Details_Should_Not_Insert_Into_Cosmos()
        {
            await _cosmosDatabaseClient.SavePaymentDetails(_paymentDetails, null);

            _cosmosWrapperMock.Verify(x => x.CreateItemAsync<SavedPaymentDetails>(It.IsAny<SavedPaymentDetails>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task Given_Indentifer_And_Stored_In_Database_Should_Return_Correct_Object()
        {
            _cosmosWrapperMock.Setup(x => x.GetItemAsync<SavedPaymentDetails>(It.IsAny<Guid>())).ReturnsAsync(_savedPaymentDetails);

            var actual = await _cosmosDatabaseClient.GetPaymentDetails(Guid.NewGuid());

            Assert.That(actual.NameOnCard, Is.EqualTo(_savedPaymentDetails.NameOnCard));
            Assert.That(actual.Amount, Is.EqualTo(_savedPaymentDetails.Amount));
            Assert.That(actual.Expiry, Is.EqualTo(_savedPaymentDetails.Expiry));
        }

        [Test]
        public async Task Given_Indentifer_And_Not_Stored_In_Database_Should_Return_Correct_Object()
        {
            _cosmosWrapperMock.Setup(x => x.GetItemAsync<SavedPaymentDetails>(It.IsAny<Guid>()));

            var actual = await _cosmosDatabaseClient.GetPaymentDetails(Guid.NewGuid());

            Assert.IsNull(actual);
        }

        [Test]
        public async Task Given_Indentifer_And_Database_Errors_Should_Throw_Exception()
        {
            _cosmosWrapperMock.Setup(x => x.GetItemAsync<SavedPaymentDetails>(It.IsAny<Guid>())).Throws(new Exception());

            Assert.ThrowsAsync<Exception>(() => _cosmosDatabaseClient.GetPaymentDetails(It.IsAny<Guid>()));
        }

        [TestCase(2348723423483435334, "***5334")]
        [TestCase(23213231, "****3231")]
        [TestCase(123, "")]
        [TestCase(1242, "")]
        public void Given_Card_Number_Should_Mask(long cardNumber, string expected)
        {
            var actual = _cosmosDatabaseClient.MaskCardNumber(new SavedPaymentDetails() { CardNumber = cardNumber});

            Assert.That(actual.Contains(expected));
        }

        [TestCase(123, "**3")]
        [TestCase(12, "")]
        public void Given_Cvv_Number_Should_Masl(int cvv, string expected)
        {
            var actual = _cosmosDatabaseClient.MaskCvv(new SavedPaymentDetails { Cvv = cvv});

            Assert.That(actual.Contains(expected));
        }

    }
}

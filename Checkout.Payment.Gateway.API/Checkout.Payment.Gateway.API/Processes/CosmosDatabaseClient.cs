using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Processes
{
    public class CosmosDatabaseClient : ICosmosDatabaseClient 
    {
        private ICosmosDatabaseWrapper<SavedPaymentDetails> _cosmosDatabaseWrapper;

        public CosmosDatabaseClient(ICosmosDatabaseWrapper<SavedPaymentDetails> cosmosDatabaseWrapper)
        {
            _cosmosDatabaseWrapper = cosmosDatabaseWrapper;
        }

        public async Task<SavedPaymentDetails> GetPaymentDetails(Guid indentifer)
        {
            try
            {
                return await _cosmosDatabaseWrapper.GetItemAsync<SavedPaymentDetails>(indentifer);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return null;
        }

        public async Task SavePaymentDetails(PaymentDetails paymentDetails, BankResponse bankResponse)
        {
            if(paymentDetails == null || bankResponse == null)
            {
                return;
            }

            var savedPaymentDetails = new SavedPaymentDetails
            {
                Amount = paymentDetails.Amount,
                CardNumber = paymentDetails.CardNumber,
                Currency = paymentDetails.Currency,
                Cvv = paymentDetails.Cvv,
                Expiry = paymentDetails.Expiry,
                Identifier = bankResponse.Identifier,
                IsSuccessful = bankResponse.PaymentSuccessful
            };

            try
            {
                await _cosmosDatabaseWrapper.CreateItemAsync<SavedPaymentDetails>(savedPaymentDetails, savedPaymentDetails.Currency);
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

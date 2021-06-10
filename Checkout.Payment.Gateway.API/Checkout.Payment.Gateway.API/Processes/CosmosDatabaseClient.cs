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

        public async Task<SavedPaymentDetailsResponse> GetPaymentDetails(Guid indentifer)
        {
            try
            {
                var response = await _cosmosDatabaseWrapper.GetItemAsync<SavedPaymentDetails>(indentifer);

                if (response != null)
                {
                    return new SavedPaymentDetailsResponse
                    {
                        CardNumber = MaskCardNumber(response),
                        Currency = response.Currency,
                        Cvv = MaskCvv(response),
                        NameOnCard = response.NameOnCard,
                        Amount = response.Amount,
                        Expiry = response.Expiry,
                        Identifier = response.Identifier, 
                        PostCode = response.PostCode
                    };
                }

                return null;
            }catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
                NameOnCard = paymentDetails.NameOnCard,
                CardNumber = paymentDetails.CardNumber,
                Currency = paymentDetails.Currency,
                Cvv = paymentDetails.Cvv,
                Expiry = paymentDetails.Expiry,
                IsSuccessful = bankResponse.PaymentSuccessful,
                Identifier = paymentDetails.Identifier,
                PostCode = paymentDetails.PostCode
            };

            try
            {
                await _cosmosDatabaseWrapper.CreateItemAsync<SavedPaymentDetails>(savedPaymentDetails, savedPaymentDetails.Currency);
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string MaskCardNumber(SavedPaymentDetails paymentDetails)
        {
            if(paymentDetails.CardNumber.ToString().Length <= 4)
            {
                return string.Empty;
            }

            var numbersToMask = paymentDetails.CardNumber.ToString().Length - 4;

            var mask = Mask(numbersToMask);

            return mask += paymentDetails.CardNumber.ToString().Substring(numbersToMask);
        }

        public string MaskCvv(SavedPaymentDetails paymentDetails)
        {
            if (paymentDetails.Cvv.ToString().Length <= 1)
            {
                return string.Empty;
            }

            var numbersToMask = paymentDetails.Cvv.ToString().Length - 1;

            var mask = Mask(numbersToMask);

            return mask += paymentDetails.Cvv.ToString().Substring(numbersToMask);
        }

        private string Mask(int lengthToMask)
        {
            var cardNumber = string.Empty;

            for (var i = 0; i < lengthToMask; i++)
            {
                cardNumber += "*";
            }

            return cardNumber;
        }
    }
}

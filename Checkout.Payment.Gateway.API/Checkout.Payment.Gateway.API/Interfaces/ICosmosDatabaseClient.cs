using Checkout.Payment.Gateway.Contracts;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Interfaces
{
    public interface ICosmosDatabaseClient
    {
        Task SavePaymentDetails(PaymentDetails paymentDetails, BankResponse bankResponse);
        Task<SavedPaymentDetailsResponse> GetPaymentDetails(Guid indentifer);
        string MaskCardNumber(SavedPaymentDetails paymentDetails);
        string MaskCvv(SavedPaymentDetails paymentDetails);
    }
}

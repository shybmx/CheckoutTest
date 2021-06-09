using Checkout.Payment.Gateway.Contracts;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Interfaces
{
    public interface ICosmosDatabaseClient
    {
        Task SavePaymentDetails(PaymentDetails paymentDetails, BankResponse bankResponse);
        Task<SavedPaymentDetails> GetPaymentDetails(Guid indentifer);
    }
}

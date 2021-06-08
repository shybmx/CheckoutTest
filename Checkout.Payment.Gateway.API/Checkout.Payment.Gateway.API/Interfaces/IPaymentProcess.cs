using Checkout.Payment.Gateway.Contracts;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Interfaces
{
    public interface IPaymentProcess
    {
        Task<BankResponse> SendPayment(PaymentDetails paymentDetails);
        Task<PaymentDetails> GetPaymentDetails(Guid identifer);
    }
}

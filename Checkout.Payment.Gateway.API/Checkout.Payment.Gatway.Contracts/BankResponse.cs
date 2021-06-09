using System;
 
namespace Checkout.Payment.Gateway.Contracts
{
    public class BankResponse
    {
        public Guid Identifier { get; set; }
        public bool PaymentSuccessful { get; set; }
    }
}

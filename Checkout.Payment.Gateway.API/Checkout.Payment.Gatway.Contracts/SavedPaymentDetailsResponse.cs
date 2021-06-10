using System;

namespace Checkout.Payment.Gateway.Contracts
{
    public class SavedPaymentDetailsResponse
    {
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public DateTime Expiry { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
        public Guid Identifier { get; set; }
    }
}

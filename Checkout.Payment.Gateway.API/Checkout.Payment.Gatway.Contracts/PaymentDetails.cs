using System;

namespace Checkout.Payment.Gateway.Contracts
{
    public class PaymentDetails
    {
        //This Id is for cosmos DB
        public string id => Identifier.ToString();
        public long CardNumber { get; set; }
        public DateTime Expiry { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public int Cvv { get; set; }
        public Guid Identifier => new Guid();
    }
}

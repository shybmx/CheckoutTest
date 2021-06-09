using Newtonsoft.Json;
using System;

namespace Checkout.Payment.Gateway.Contracts
{
    public class PaymentDetails
    {
        //This Id is for cosmos DB
        public string id => Identifier.ToString();
        [JsonProperty(Required = Required.Always)]
        [JsonProperty()]
        public long CardNumber { get; set; }
        [JsonProperty(Required = Required.Always)]
        public DateTime Expiry { get; set; }
        [JsonProperty(Required = Required.Always)]
        public double Amount { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Currency { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int Cvv { get; set; }
        public Guid Identifier { get; set; }
    }
}

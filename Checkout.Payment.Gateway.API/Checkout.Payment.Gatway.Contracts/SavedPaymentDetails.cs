using Newtonsoft.Json;
using System;

namespace Checkout.Payment.Gateway.Contracts
{
    public class SavedPaymentDetails : PaymentDetails
    {
        //This Id is for cosmos DB
        [JsonProperty(PropertyName = "id")]
        private string Id => Identifier.ToString();
        public bool IsSuccessful { get; set; }
        public Guid Identifier { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Payment.Gateway.Contracts
{
    public class PaymentDetails
    {
        //This Id is for cosmos DB
        [JsonProperty(PropertyName = "id")]
        private string Id => Identifier.ToString();
        [Required]
        [Range(11111111111111, 99999999999999)]
        public long CardNumber { get; set; }
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public DateTime Expiry { get; set; }
        [Required]
        [Range(0.1, 999999.99)]
        public double Amount { get; set; }
        [Required]
        [StringLength(5)]
        public string Currency { get; set; }
        [Required]
        [Range(100,999)]
        public int Cvv { get; set; }
        public Guid Identifier { get; set; }
    }
}

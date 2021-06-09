namespace Checkout.Payment.Gateway.Contracts
{
    public class SavedPaymentDetails : PaymentDetails
    {
        public bool IsSuccessful { get; set; }
    }
}

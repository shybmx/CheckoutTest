namespace Checkout.Payment.Gateway.Contracts
{
    public class CosmosConfiguration
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitonKey { get; set; }
    }
}

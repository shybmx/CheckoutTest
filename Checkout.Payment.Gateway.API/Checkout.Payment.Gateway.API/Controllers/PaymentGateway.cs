using Microsoft.AspNetCore.Mvc;

namespace Checkout.Payment.Gateway.API.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentGateway : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] PaymentGateway paymentGateway)
        {
            if(paymentGateway == null)
            {
                return new BadRequestObjectResult("Invalid Query");
            }

            return null;
        }
    }
}

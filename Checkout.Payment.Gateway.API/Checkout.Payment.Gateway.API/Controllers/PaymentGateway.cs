using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentGateway : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ProcessPayment([FromQuery] PaymentDetails paymentDetails)
        {
            if(paymentDetails == null)
            {
                return new BadRequestObjectResult("Invalid Query");
            }



            return new OkObjectResult("");
        }
    }
}

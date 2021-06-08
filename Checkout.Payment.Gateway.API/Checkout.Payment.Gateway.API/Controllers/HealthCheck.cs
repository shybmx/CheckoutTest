using Microsoft.AspNetCore.Mvc;

namespace Checkout.Payment.Gateway.API.Controllers
{
    [ApiController]
    [Route("HealthCheck")]
    public class HealthCheck : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealthCheck()
        {
            return new OkObjectResult("Healthy");
        }
    }
}

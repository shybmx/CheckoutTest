using Microsoft.AspNetCore.Mvc;

namespace Checkout.Payment.Gateway.API.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }
    }
}

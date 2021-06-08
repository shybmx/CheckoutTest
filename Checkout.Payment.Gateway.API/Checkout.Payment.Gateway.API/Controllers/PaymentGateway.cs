using Checkout.Payment.Gateway.API.Interfaces;
using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Gateway.API.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentGateway : ControllerBase
    {
        private IPaymentProcess _paymentProcess;

        public PaymentGateway(IPaymentProcess paymentProcess)
        {
            _paymentProcess = paymentProcess;
        }

        [HttpGet]
        public async Task<IActionResult> ProcessPayment([FromQuery] PaymentDetails paymentDetails)
        {
            if(paymentDetails == null)
            {
                return new BadRequestObjectResult("Invalid Query");
            }

            var details = await _paymentProcess.SendPayment(paymentDetails);

            if (details != null)
            {
                return new OkObjectResult(details);
            }

            return new NotFoundObjectResult("");
        }


        [HttpGet]
        public async Task<IActionResult> GetPaymentDetails([FromQuery] Guid identifer)
        {
            var paymentDetails = await _paymentProcess.GetPaymentDetails(identifer);

            return new OkObjectResult(paymentDetails);
        }
    }
}

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
        private ICosmosDatabaseClient _cosmosDatabaseClient;

        public PaymentGateway(IPaymentProcess paymentProcess, ICosmosDatabaseClient cosmosDatabaseClient)
        {
            _paymentProcess = paymentProcess;
            _cosmosDatabaseClient = cosmosDatabaseClient;
        }

        [HttpGet("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromQuery] PaymentDetails paymentDetails)
        {
            if(paymentDetails == null)
            {
                return new BadRequestObjectResult("Invalid Query");
            }

            var details = await _paymentProcess.SendPayment(paymentDetails);

            await _cosmosDatabaseClient.SavePaymentDetails(paymentDetails, details);

            if (details != null)
            {
                return new OkObjectResult(details);
            }

            return new NotFoundObjectResult("Cannot Communicate with Bank");
        }


        [HttpGet("GetPaymentDetails")]
        public async Task<IActionResult> GetPaymentDetails([FromQuery] Guid identifer)
        {
            var paymentDetails = await _cosmosDatabaseClient.GetPaymentDetails(identifer);

            if(paymentDetails != null)
            {
                return new OkObjectResult(paymentDetails);  
            }

            return new NotFoundObjectResult($"Cannot find payment details for {identifer}");
        }
    }
}

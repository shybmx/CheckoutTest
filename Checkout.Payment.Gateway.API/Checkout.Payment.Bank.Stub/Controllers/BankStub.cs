using Checkout.Payment.Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Checkout.Payment.Bank.Stub.Controllers
{
    [ApiController]
    [Route("Banking")]
    public class BankStub : ControllerBase
    {
        [HttpPost("executePurchase")]
        public async Task<IActionResult> BankExecutePurchase([FromBody] PaymentDetails paymentDetails)
        {
            var isSuccessful = false;

            var randomNumber = new Random();
            if(randomNumber.Next(0, 100) % 0 == 0)
            {
                isSuccessful = true;
            }

            return new OkObjectResult(new BankResponse { Identifier = paymentDetails.Identifier, PaymentSuccessful = isSuccessful });
        }
    }
}

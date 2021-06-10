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
            var isSuccessful = true;

            var randomNumber = new Random();
            if(randomNumber.Next(0, 100) % 3 == 0)
            {
                isSuccessful = false;
            }

            return new OkObjectResult(new BankResponse { Identifier = Guid.NewGuid(), PaymentSuccessful = isSuccessful });
        }
    }
}

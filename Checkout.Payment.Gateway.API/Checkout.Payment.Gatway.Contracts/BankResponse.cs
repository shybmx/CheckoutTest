using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Payment.Gatway.Contracts
{
    public class BankResponse
    {
        public Guid Identifier { get; set; }
        public bool PaymentSuccessful { get; set; }
    }
}

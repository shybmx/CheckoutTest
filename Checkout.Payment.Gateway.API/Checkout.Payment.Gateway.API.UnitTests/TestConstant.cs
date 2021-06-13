using System;

namespace Checkout.Payment.Gateway.API.UnitTests
{
    public static class TestConstant
    {
        public static long CardNumber = 2348723423483435334;
        public static DateTime Expiry = DateTime.UtcNow;
        public static double Amount = 34.26;
        public static string Currency = "GBP";
        public static int Cvv = 221;
    }
}

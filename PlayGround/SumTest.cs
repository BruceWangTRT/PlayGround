using System;
using System.Collections.Generic;

namespace PlayGround
{
    public class SumTest
    {
        public List<decimal> NightlyRates { get; set; }
        public SumTest()
        {
            NightlyRates = new List<decimal>();
        }

        public void Withdraw(double amount)
        {
            if (NightlyRates.Count >= amount)
            {
                Console.WriteLine("success");
            }
            else
            {
                throw new ArgumentException("Withdrawal exceeds balance!");
            }
        }
    }
}

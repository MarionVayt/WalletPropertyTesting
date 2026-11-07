using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public readonly struct Money
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Money cannot be negative.");
            Amount = amount;
        }

        public static Money operator +(Money a, Money b)
            => new Money(a.Amount + b.Amount);

        public static Money operator -(Money a, Money b)
        {
            if (a.Amount < b.Amount)
                throw new InvalidOperationException("Insufficient funds");
            return new Money(a.Amount - b.Amount);
        }

        public override string ToString() => Amount.ToString("F2");
    }
}

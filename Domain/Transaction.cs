using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public class Transaction
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public Money Amount { get; }
        public TransactionType Type { get; }

        private Transaction(Money amount, TransactionType type)
        {
            Amount = amount;
            Type = type;
        }

        public static Transaction Deposit(Money amount) =>
            new Transaction(amount, TransactionType.Deposit);

        public static Transaction Withdraw(Money amount) =>
            new Transaction(amount, TransactionType.Withdraw);
    }

    public enum TransactionType
    {
        Deposit,
        Withdraw
    }

}

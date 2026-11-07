using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public class Wallet
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Money Balance { get; private set; } = new Money(0);
        public List<Transaction> History { get; } = new();

        public void Deposit(Money amount)
        {
            Balance += amount;
            History.Add(Transaction.Deposit(amount));
        }

        public void Withdraw(Money amount)
        {
            Balance -= amount;
            History.Add(Transaction.Withdraw(amount));
        }
    }

}

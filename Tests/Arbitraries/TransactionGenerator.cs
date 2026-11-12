using FsCheck;
using FsCheck.Fluent;
using WalletPropertyTesting.Domain;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class TransactionGenerator
    {
        // Транзакція = сума (з MoneyGenerator) + тип (Deposit / Withdraw).
        // Конструктор Transaction закритий, тому створюємо через фабричні методи.
        public static Arbitrary<Transaction> Transaction()
        {
            Gen<Transaction> gen =
                from money in MoneyGenerator.Money().Generator
                from type in Gen.Elements(TransactionType.Deposit, TransactionType.Withdraw)
                select type == TransactionType.Deposit
                    ? WalletPropertyTesting.Domain.Transaction.Deposit(money)
                    : WalletPropertyTesting.Domain.Transaction.Withdraw(money);

            return gen.ToArbitrary();
        }
    }
}
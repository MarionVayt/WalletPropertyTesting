using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck.Fluent;
using WalletPropertyTesting.Domain;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class TransactionGenerator
    {
        // Транзакція = сума + тип (як VehicleGen будується з FuelTankGen):
        // використайте MoneyGenerator.Money().Generator і Gen.Elements(TransactionType.Deposit, TransactionType.Withdraw).
        // Конструктор Transaction закритий: створюйте через Transaction.Deposit(money) або Transaction.Withdraw(money).
        public static Arbitrary<Transaction> Transaction()
        {
            throw new NotImplementedException();

        }
    }

}

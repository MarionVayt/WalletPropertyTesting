using FluentAssertions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{
    public class WalletProperties
    {
        // P1 · Туди й назад: Deposit → Withdraw тієї ж суми повертає баланс як був
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Deposit_then_withdraw_restores_balance(Money[] initial, Money amount)
        {
            var wallet = new Wallet();
            foreach (var m in initial)
                wallet.Deposit(m);
            decimal before = wallet.Balance.Amount;

            wallet.Deposit(amount);
            wallet.Withdraw(amount);

            wallet.Balance.Amount.Should().Be(before);
        }

        // P2 · Інваріант: у History потрапляють лише успішні операції,
        // а невдале зняття не змінює баланс
        [Property(Arbitrary = new[] { typeof(MoneyGenerator), typeof(TransactionGenerator) })]
        public bool History_has_only_successful_operations(Transaction[] operations)
        {
            var wallet = new Wallet();
            int successful = 0;
            bool balanceUnchangedOnFailure = true;

            foreach (var op in operations)
            {
                decimal before = wallet.Balance.Amount;
                try
                {
                    if (op.Type == TransactionType.Deposit)
                        wallet.Deposit(op.Amount);
                    else
                        wallet.Withdraw(op.Amount);
                    successful++;
                }
                catch (InvalidOperationException)
                {
                    balanceUnchangedOnFailure &= wallet.Balance.Amount == before;
                }
            }

            return balanceUnchangedOnFailure && wallet.History.Count == successful;
        }

        // P7 · Легко перевірити: баланс = сума внесених сум (беремо з вхідного масиву)
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Balance_equals_sum_of_deposited_amounts(Money[] amounts)
        {
            var wallet = new Wallet();
            foreach (var m in amounts)
                wallet.Deposit(m);

            wallet.Balance.Amount.Should().Be(amounts.Sum(x => x.Amount));
        }

        // Тест винятку InvalidOperationException + передумова When:
        // зняти більше, ніж є на балансі
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public Property Withdraw_more_than_balance_throws_when(Money deposit, Money amount)
        {
            var wallet = new Wallet();
            wallet.Deposit(deposit);
            decimal before = wallet.Balance.Amount;

            bool applicable = amount.Amount > before;
            bool ok = true;

            if (applicable)
            {
                Action act = () => wallet.Withdraw(amount);
                act.Should().Throw<InvalidOperationException>();

                ok = wallet.Balance.Amount == before
                     && wallet.History.Count == 1;
            }

            return ok.When(applicable);
        }
        

        // P8 · Без винятків: Deposit будь-якої невід'ємної суми не кидає виняток
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Deposit_of_any_amount_does_not_throw(Money[] initial, Money amount)
        {
            var wallet = new Wallet();
            foreach (var m in initial)
                wallet.Deposit(m);

            Action act = () => wallet.Deposit(amount);

            act.Should().NotThrow();
        }
    }
}
using FluentAssertions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{
    public class RepositoryProperties
    {
        // P5 · Оракул: WalletService дає той самий баланс, що прямі виклики Wallet.Deposit
        // (кожен шлях має свій окремий гаманець)
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Service_gives_same_balance_as_direct_wallet(Money[] amounts)
        {
            var service = new WalletService(new InMemoryWalletRepository());
            var viaService = service.CreateWallet();
            var direct = new Wallet();

            foreach (var m in amounts)
            {
                service.Deposit(viaService.Id, m.Amount);
                direct.Deposit(m);
            }

            service.GetBalance(viaService.Id).Amount.Should().Be(direct.Balance.Amount);
        }

        // Тест винятку: KeyNotFoundException (гаманця з таким Id немає)
        [Property]
        public void Unknown_wallet_throws(Guid id)
        {
            var service = new WalletService(new InMemoryWalletRepository());

            Action act = () => service.GetBalance(id);

            act.Should().Throw<KeyNotFoundException>();
        }

        // P3 · Ідемпотентність: повторний Save того самого гаманця нічого не змінює
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Saving_same_wallet_twice_keeps_one_copy(Money[] amounts)
        {
            var repo = new InMemoryWalletRepository();

            foreach (var m in amounts)
            {
                var wallet = new Wallet();
                wallet.Deposit(m);
                repo.Save(wallet);
                repo.Save(wallet);
            }

            repo.All().Count().Should().Be(amounts.Length);
        }

        // Тест винятку: ArgumentException (внесення від'ємної суми через сервіс),
        // баланс при цьому не змінюється
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Deposit_negative_amount_via_service_throws(Money seed)
        {
            var service = new WalletService(new InMemoryWalletRepository());
            var wallet = service.CreateWallet();
            decimal negative = -(seed.Amount + 0.01m);

            Action act = () => service.Deposit(wallet.Id, negative);

            act.Should().Throw<ArgumentException>();
            service.GetBalance(wallet.Id).Amount.Should().Be(0m);
        }
    }
}
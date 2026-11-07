using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public class WalletService
    {
        private readonly IWalletRepository _repo;

        public WalletService(IWalletRepository repo)
        {
            _repo = repo;
        }

        public Wallet CreateWallet()
        {
            var wallet = new Wallet();
            _repo.Save(wallet);
            return wallet;
        }

        public void Deposit(Guid walletId, decimal amount)
        {
            var wallet = _repo.Get(walletId);
            wallet.Deposit(new Money(amount));
            _repo.Save(wallet);
        }

        public void Withdraw(Guid walletId, decimal amount)
        {
            var wallet = _repo.Get(walletId);
            wallet.Withdraw(new Money(amount));
            _repo.Save(wallet);
        }

        public Money GetBalance(Guid walletId)
            => _repo.Get(walletId).Balance;
    }

}

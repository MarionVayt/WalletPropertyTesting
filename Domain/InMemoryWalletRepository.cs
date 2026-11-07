using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private readonly Dictionary<Guid, Wallet> _storage = new();

        public Wallet Get(Guid id)
        {
            if (!_storage.ContainsKey(id))
                throw new KeyNotFoundException("Wallet not found");

            return _storage[id];
        }

        public void Save(Wallet wallet)
        {
            _storage[wallet.Id] = wallet;
        }

        public IEnumerable<Wallet> All() => _storage.Values;
    }

}

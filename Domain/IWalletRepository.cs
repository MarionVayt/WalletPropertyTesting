using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletPropertyTesting.Domain
{
    public interface IWalletRepository
    {
        Wallet Get(Guid id);
        void Save(Wallet wallet);
        IEnumerable<Wallet> All();
    }

}

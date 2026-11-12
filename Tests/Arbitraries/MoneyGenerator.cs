using FsCheck;
using FsCheck.Fluent;
using WalletPropertyTesting.Domain;

namespace WalletPropertyTesting.Tests.Arbitraries
{
    public static class MoneyGenerator
    {
        // Money: Amount >= 0, дві цифри після коми.
        // Приблизно 1/4 значень — граничні (0, 0.01, 1, 1000), решта — випадкові від 0.00 до 1000.00.
        public static Arbitrary<Money> Money()
        {
            Gen<decimal> edge = Gen.Elements(0m, 0.01m, 1m, 1000m);
            Gen<decimal> regular = Gen.Choose(0, 100_000).Select(cents => cents / 100m);

            Gen<Money> gen = Gen.OneOf(edge, regular, regular, regular)
                .Select(amount => new Money(amount));

            return gen.ToArbitrary();
        }
    }
}
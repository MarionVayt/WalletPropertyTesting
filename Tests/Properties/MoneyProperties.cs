using FluentAssertions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using WalletPropertyTesting.Domain;
using WalletPropertyTesting.Tests.Arbitraries;

namespace WalletPropertyTesting.Tests.Properties
{
    public class MoneyProperties
    {
        // Тест винятку: ArgumentException (від'ємна сума)
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Negative_amount_throws(Money seed)
        {
            decimal negative = -(seed.Amount + 0.01m);

            Action act = () => new Money(negative);

            act.Should().Throw<ArgumentException>();
        }

        // P1 · Туди й назад: (a + b) - b = a
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public void Add_then_subtract_returns_original(Money a, Money b)
        {
            Money result = (a + b) - b;

            result.Amount.Should().Be(a.Amount);
        }

        // P2 · Інваріант, з передумовою When: якщо a >= b, різниця в межах [0, a]
        [Property(Arbitrary = new[] { typeof(MoneyGenerator) })]
        public Property Subtraction_stays_between_zero_and_minuend_when_enough(Money a, Money b)
        {
            bool applicable = a.Amount >= b.Amount;
            bool ok = true;

            if (applicable)
            {
                Money result = a - b;
                ok = result.Amount >= 0m && result.Amount <= a.Amount;
            }

            return ok.When(applicable);
        }
    }
}
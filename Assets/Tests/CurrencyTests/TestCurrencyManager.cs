using BreakInfinity;
using Currency;
using Enums;
using Helpers;
using NUnit.Framework;

namespace Tests.CurrencyTests
{
    public class test_currency_manager
    {
        [Test]
        public void Should_Controllers_Initiated_With_Correct_Amount()
        {
            var manager = new CurrencyManager();

            Assert.AreEqual(Constants.MoneyStartValue, manager.GetCurrentAmount(CurrencyType.Money).ToDouble());
            Assert.AreEqual(Constants.GemStartValue, manager.GetCurrentAmount(CurrencyType.Gem).ToDouble());
        }

        [Test]
        public void Should_Controllers_Add_Correct_Amount()
        {
            var manager = new CurrencyManager();
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);
            BigDouble expectedMoney = moneyToAdd + Constants.MoneyStartValue;
            BigDouble expectedGem = gemToAdd + Constants.GemStartValue;

            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);

            Assert.AreEqual(expectedMoney, manager.GetCurrentAmount(CurrencyType.Money));
            Assert.AreEqual(expectedGem, manager.GetCurrentAmount(CurrencyType.Gem));
        }

        [Test]
        public void Should_Controllers_Subtract_Correct_Amount()
        {
            var manager = new CurrencyManager();
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);
            BigDouble moneyToSubtract = new BigDouble(2, 1);
            BigDouble gemToSubtract = new BigDouble(1, 0);
            BigDouble expectedMoney = moneyToAdd + Constants.MoneyStartValue - moneyToSubtract;
            BigDouble expectedGem = gemToAdd + Constants.GemStartValue - gemToSubtract;

            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);
            manager.SubtractAmount(CurrencyType.Money, moneyToSubtract);
            manager.SubtractAmount(CurrencyType.Gem, gemToSubtract);

            Assert.AreEqual(expectedMoney, manager.GetCurrentAmount(CurrencyType.Money));
            Assert.AreEqual(expectedGem, manager.GetCurrentAmount(CurrencyType.Gem));
        }

        [Test]
        public void Should_Controllers_Has_Enough_Amount()
        {
            var manager = new CurrencyManager();
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);

            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);

            bool isEqualMoneyCountAsEnough = manager.HasEnoughAmount(CurrencyType.Money, moneyToAdd);
            bool isEqualGemCountAsEnough = manager.HasEnoughAmount(CurrencyType.Gem, gemToAdd);
            bool isMoneyCountAsEnough = manager.HasEnoughAmount(CurrencyType.Money, moneyToAdd / 2);
            bool isGemCountAsEnough = manager.HasEnoughAmount(CurrencyType.Gem, gemToAdd / 2);

            Assert.IsTrue(isEqualMoneyCountAsEnough, "Equal values should act as enough.");
            Assert.IsTrue(isEqualGemCountAsEnough, "Equal values should act as enough.");
            Assert.IsTrue(isMoneyCountAsEnough, "Values less than current money should be enough.");
            Assert.IsTrue(isGemCountAsEnough, "Values less than current money should be enough.");
        }

        [Test]
        public void Should_Subscribe_To_Controller()
        {
            var manager = new CurrencyManager();
            int callCount = 0;
            BigDouble moneyToAdd = new BigDouble(3, 4);

            manager.SubscribeToCurrencyChanges(CurrencyType.Money, _ => callCount++);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_Subscribe_To_Selected_Controller()
        {
            var manager = new CurrencyManager();
            int callCount = 0;
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);

            manager.SubscribeToCurrencyChanges(CurrencyType.Gem, _ => callCount++);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_UnSubscribe_From_Controller()
        {
            var manager = new CurrencyManager();
            int callCount = 0;
            BigDouble moneyToAdd = new BigDouble(3, 4);

            void SubscribeAction(BigDouble temp)
            {
                callCount++;
            }

            manager.SubscribeToCurrencyChanges(CurrencyType.Money, SubscribeAction);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.UnSubscribeFromCurrencyChanges(CurrencyType.Money, SubscribeAction);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_UnSubscribe_From_All()
        {
            var manager = new CurrencyManager();
            int callCount = 0;
            BigDouble moneyToAdd = new BigDouble(3, 4);

            void SubscribeAction(BigDouble temp)
            {
                callCount++;
            }

            manager.SubscribeToCurrencyChanges(CurrencyType.Money, SubscribeAction);
            manager.SubscribeToCurrencyChanges(CurrencyType.Gem, SubscribeAction);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, moneyToAdd);
            manager.RemoveAllListeners();
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, moneyToAdd);

            Assert.AreEqual(2, callCount);
        }
    }
}
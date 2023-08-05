using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Currency;
using Enums;
using Installers;
using NUnit.Framework;
using Zenject;

namespace Tests.CurrencyTests
{
    [TestFixture]
    public class TestCurrencyManager : ZenjectUnitTestFixture
    {
        private const string SettingsInstallerPath = "Installers/SettingsInstaller";
        [Inject] private List<CurrencyData> _currencyDatas;

        [SetUp]
        public void BindSettings()
        {
            Container.Bind(typeof(CurrencyManager)).AsTransient();
            SignalBusInstaller.Install(Container);
            SettingsInstaller.InstallFromResource(SettingsInstallerPath, Container);

            Container.DeclareSignal<CurrencyChangedSignal>();
            Container.Inject(this);
        }

        [Test]
        public void Should_Controllers_Initiated_With_Correct_Amount()
        {
            var manager = Container.Resolve<CurrencyManager>();

            var moneyStartValue = _currencyDatas.FirstOrDefault(x => x.currencyType == CurrencyType.Money).currentAmount;
            var gemStartValue = _currencyDatas.FirstOrDefault(x => x.currencyType == CurrencyType.Gem).currentAmount;

            Assert.AreEqual(moneyStartValue, manager.GetCurrentAmount(CurrencyType.Money));
            Assert.AreEqual(gemStartValue, manager.GetCurrentAmount(CurrencyType.Gem));
        }

        [Test]
        public void Should_Controllers_Add_Correct_Amount()
        {
            var manager = Container.Resolve<CurrencyManager>();
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);
            BigDouble expectedMoney = moneyToAdd + manager.GetCurrentAmount(CurrencyType.Money);
            BigDouble expectedGem = gemToAdd + manager.GetCurrentAmount(CurrencyType.Gem);

            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);

            Assert.AreEqual(expectedMoney, manager.GetCurrentAmount(CurrencyType.Money));
            Assert.AreEqual(expectedGem, manager.GetCurrentAmount(CurrencyType.Gem));
        }

        [Test]
        public void Should_Controllers_Subtract_Correct_Amount()
        {
            var manager = Container.Resolve<CurrencyManager>();
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);
            BigDouble moneyToSubtract = new BigDouble(2, 1);
            BigDouble gemToSubtract = new BigDouble(1, 0);
            BigDouble expectedMoney = moneyToAdd + manager.GetCurrentAmount(CurrencyType.Money) - moneyToSubtract;
            BigDouble expectedGem = gemToAdd + manager.GetCurrentAmount(CurrencyType.Gem) - gemToSubtract;

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
            var manager = Container.Resolve<CurrencyManager>();
            BigDouble startMoney = manager.GetCurrentAmount(CurrencyType.Money);
            BigDouble startGem = manager.GetCurrentAmount(CurrencyType.Gem);
            BigDouble moneyToAdd = new BigDouble(3, 4);
            BigDouble gemToAdd = new BigDouble(3, 0);

            manager.AddAmount(CurrencyType.Money, moneyToAdd);
            manager.AddAmount(CurrencyType.Gem, gemToAdd);

            bool isEqualMoneyCountAsEnough = manager.HasEnoughAmount(CurrencyType.Money, moneyToAdd + startMoney);
            bool isEqualGemCountAsEnough = manager.HasEnoughAmount(CurrencyType.Gem, gemToAdd + startGem);
            bool isMoneyCountAsEnough = manager.HasEnoughAmount(CurrencyType.Money, moneyToAdd / 2);
            bool isGemCountAsEnough = manager.HasEnoughAmount(CurrencyType.Gem, gemToAdd / 2);

            Assert.IsTrue(isEqualMoneyCountAsEnough, "Equal values should act as enough.");
            Assert.IsTrue(isEqualGemCountAsEnough, "Equal values should act as enough.");
            Assert.IsTrue(isMoneyCountAsEnough, "Values less than current money should be enough.");
            Assert.IsTrue(isGemCountAsEnough, "Values less than current money should be enough.");
        }
    }
}
using System;
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
    public class TestCurrencyController : ZenjectUnitTestFixture
    {
        private const string SettingsInstallerPath = "Installers/SettingsInstaller";
        [Inject] private SignalBus _signalBus;
        [Inject] private List<CurrencyData> _currencyDatas;

        private BigDouble[] _moneyAmountsToAdd =
        {
            new(1, 1), new(8, 2), new(3, 4),
            new(2, 2), new(9, 3), new(4, 6),
            new(3, 3), new(2, 2), new(4, 5),
            new(4, 1), new(2, 5), new(4, 8),
            new(5, 2), new(2, 6), new(5, 2),
            new(6, 3), new(3, 8), new(5, 4),
            new(7, 1), new(3, 9), new(5, 5),
        };

        [SetUp]
        public void BindSettings()
        {
            SignalBusInstaller.Install(Container);
            SettingsInstaller.InstallFromResource(SettingsInstallerPath, Container);

            Container.DeclareSignal<CurrencyChangedSignal>();
            Container.Inject(this);
        }

        private CurrencyController CreateNewCurrencyController(CurrencyType currencyType, bool isMoneyOverride = false,
            BigDouble overrideMoney = default)
        {
            var data = _currencyDatas.FirstOrDefault(x => x.currencyType == currencyType);
            if (isMoneyOverride)
            {
                data.currentAmount = overrideMoney;
            }

            var controller = new CurrencyController(_signalBus, data);

            return controller;
        }

        private BigDouble GetStartValue(CurrencyType currencyType)
        {
            var data = _currencyDatas.FirstOrDefault(x => x.currencyType == currencyType);
            return data.currentAmount;
        }

        [Test]
        public void Should_Controller_Initialized()
        {
            CurrencyType currencyType = CurrencyType.Money;
            BigDouble moneyAmount = GetStartValue(currencyType);

            var controller = CreateNewCurrencyController(currencyType);

            Assert.AreEqual(currencyType, controller.CurrencyType);
            Assert.AreEqual(moneyAmount.ToDouble(), GetStartValue(currencyType).ToDouble());
        }

        [Test]
        public void Should_Controller_Add_Money()
        {
            CurrencyType currencyType = CurrencyType.Money;
            BigDouble moneyAmount = GetStartValue(currencyType);
            BigDouble expectedResult = moneyAmount;

            var controller = CreateNewCurrencyController(currencyType);

            foreach (var amountToAdd in _moneyAmountsToAdd)
            {
                expectedResult += amountToAdd;
                controller.AddAmount(amountToAdd);
                Assert.AreEqual(expectedResult, controller.CurrentAmount);
            }
        }

        [Test]
        public void Should_Controller_Subtract_Money()
        {
            CurrencyType currencyType = CurrencyType.Money;
            BigDouble expectedResult = 0;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            foreach (var amountToAdd in _moneyAmountsToAdd)
            {
                expectedResult += amountToAdd;
                controller.AddAmount(amountToAdd);
            }

            foreach (var amountToSubtract in _moneyAmountsToAdd)
            {
                expectedResult -= amountToSubtract;
                controller.SubtractAmount(amountToSubtract);
                Assert.AreEqual(expectedResult, controller.CurrentAmount);
            }

            Assert.AreEqual(BigDouble.Zero, controller.CurrentAmount);
        }

        [Test]
        public void Should_Controller_Not_Subtract_Unless_Afford()
        {
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.SubtractAmount(value);
                Assert.GreaterOrEqual(controller.CurrentAmount, BigDouble.Zero,
                    $"Controller started with 0 and extracted by {value}");
            }

            BigDouble startMoney = _moneyAmountsToAdd[3];
            controller.AddAmount(startMoney);

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.SubtractAmount(value);
                Assert.GreaterOrEqual(controller.CurrentAmount, BigDouble.Zero,
                    $"Controller started with {startMoney} and extracted by {value}");
            }
        }

        [Test]
        public void Should_Controller_Afford()
        {
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, _moneyAmountsToAdd.Max());

            foreach (var value in _moneyAmountsToAdd)
            {
                bool canAfford = controller.HasEnoughAmount(value);
                Assert.IsTrue(canAfford, "Controller should have afford it.");
            }
        }

        [Test]
        public void Should_Controller_Not_Deal_With_Negative_Values()
        {
            BigDouble startValue = _moneyAmountsToAdd.Max();
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, startValue);

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.AddAmount(value * -1);
                Assert.AreEqual(startValue, controller.CurrentAmount);
            }

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.SubtractAmount(value * -1);
                Assert.AreEqual(startValue, controller.CurrentAmount);
            }
        }

        [Test]
        public void Should_Controller_Not_Afford()
        {
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            foreach (var value in _moneyAmountsToAdd)
            {
                bool canAfford = controller.HasEnoughAmount(value);
                Assert.IsFalse(canAfford, "Controller shouldn't afford it.");
            }
        }

        [Test]
        public void Should_Controller_Notify_Only_Once_On_Adding()
        {
            int callCount = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType);

            Action eventCallback = () => callCount++;
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallback);

            controller.AddAmount(50);

            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallback);
            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_Controller_Notify_Only_Once_On_Subtracting()
        {
            int callCount = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 50);

            Action eventCallback = () => callCount++;
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallback);

            controller.SubtractAmount(50);

            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallback);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_Controller_Notify_For_Every_Value_Change()
        {
            int callCount = 0;
            int expectedCount = _moneyAmountsToAdd.Length * 2;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 50);
            Action eventCallback = () => callCount++;
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallback);

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.AddAmount(value);
                controller.SubtractAmount(value);
            }

            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallback);

            Assert.AreEqual(expectedCount, callCount);
        }

        [Test]
        public void Should_Controller_Notify_Only_If_Amount_Changed()
        {
            int callCount = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);
            Action eventCallback = () => callCount++;
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallback);
            controller.SubtractAmount(50);
            controller.SubtractAmount(-50);
            controller.AddAmount(-50);

            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallback);

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void Should_Controller_Notify_On_Adding()
        {
            BigDouble moneyFromAction = 0;
            BigDouble addedMoney = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            Action<CurrencyChangedSignal> eventCallback = currencyData =>
            {
                moneyFromAction = currencyData.CurrencyData.currentAmount;
            };

            _signalBus.Subscribe(eventCallback);
            // controller.OnAmountChanged += value => moneyFromAction = value;

            foreach (var moneyToAdd in _moneyAmountsToAdd)
            {
                addedMoney += moneyToAdd;
                controller.AddAmount(moneyToAdd);
                Assert.AreEqual(addedMoney, moneyFromAction);
            }

            _signalBus.Unsubscribe(eventCallback);
        }

        [Test]
        public void Should_Controller_Notify_On_Subtracting()
        {
            BigDouble moneyFromAction = 0;
            BigDouble subtractedMoney = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            foreach (var amountToAdd in _moneyAmountsToAdd)
            {
                subtractedMoney += amountToAdd;
                controller.AddAmount(amountToAdd);
            }

            Action<CurrencyChangedSignal> eventCallback = currencyData =>
            {
                moneyFromAction = currencyData.CurrencyData.currentAmount;
            };

            _signalBus.Subscribe(eventCallback);
            // controller.OnAmountChanged += value => moneyFromAction = value;

            foreach (var moneyToSubtract in _moneyAmountsToAdd)
            {
                subtractedMoney -= moneyToSubtract;
                controller.SubtractAmount(moneyToSubtract);
                Assert.AreEqual(subtractedMoney, moneyFromAction);
            }

            _signalBus.Unsubscribe(eventCallback);
        }

        [Test]
        public void Should_Controller_Remove_All_Listeners()
        {
            int callCountToIncrease = 0;
            int callCountToDecrease = 2;
            int expectedCallCount = 1;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = CreateNewCurrencyController(currencyType, true, 0);

            Action eventCallbackIncrease = () => callCountToIncrease++;
            Action eventCallbackDecrease = () => callCountToDecrease--;
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallbackIncrease);
            _signalBus.Subscribe<CurrencyChangedSignal>(eventCallbackDecrease);

            controller.AddAmount(1);

            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallbackIncrease);
            _signalBus.Unsubscribe<CurrencyChangedSignal>(eventCallbackDecrease);

            Assert.AreEqual(expectedCallCount, callCountToIncrease);
            Assert.AreEqual(expectedCallCount, callCountToDecrease);
        }
    }
}
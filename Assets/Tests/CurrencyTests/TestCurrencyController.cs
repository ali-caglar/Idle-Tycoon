using System.Linq;
using BreakInfinity;
using Currency;
using Enums;
using Helpers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Tests.CurrencyTests
{
    public class test_currency_controller
    {
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

        [Test]
        public void Should_Controller_Initialized()
        {
            CurrencyType currencyType = CurrencyType.Money;
            BigDouble moneyAmount = Constants.MoneyStartValue;

            var controller = new CurrencyController(currencyType, moneyAmount);

            Assert.AreEqual(currencyType, controller.CurrencyType);
            Assert.AreEqual(moneyAmount.ToDouble(), Constants.MoneyStartValue);
        }

        [Test]
        public void Should_Controller_Add_Money()
        {
            CurrencyType currencyType = CurrencyType.Money;
            BigDouble moneyAmount = Constants.MoneyStartValue;
            BigDouble expectedResult = moneyAmount;

            var controller = new CurrencyController(currencyType, moneyAmount);

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

            var controller = new CurrencyController(currencyType, 0);

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

            var controller = new CurrencyController(currencyType, 0);

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

            var controller = new CurrencyController(currencyType, _moneyAmountsToAdd.Max());

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

            var controller = new CurrencyController(currencyType, startValue);

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

            var controller = new CurrencyController(currencyType, 0);

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

            var controller = new CurrencyController(currencyType, 0);
            controller.OnAmountChanged += _ => callCount++;
            controller.AddAmount(50);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_Controller_Notify_Only_Once_On_Subtracting()
        {
            int callCount = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 50);
            controller.OnAmountChanged += _ => callCount++;
            controller.SubtractAmount(50);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void Should_Controller_Notify_For_Every_Value_Change()
        {
            int callCount = 0;
            int expectedCount = _moneyAmountsToAdd.Length * 2;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 50);
            controller.OnAmountChanged += _ => callCount++;

            foreach (var value in _moneyAmountsToAdd)
            {
                controller.AddAmount(value);
                controller.SubtractAmount(value);
            }

            Assert.AreEqual(expectedCount, callCount);
        }

        [Test]
        public void Should_Controller_Notify_Only_If_Amount_Changed()
        {
            int callCount = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 0);
            controller.OnAmountChanged += _ => callCount++;
            controller.SubtractAmount(50);
            controller.SubtractAmount(-50);
            controller.AddAmount(-50);

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void Should_Controller_Notify_On_Adding()
        {
            BigDouble moneyFromAction = 0;
            BigDouble addedMoney = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 0);
            controller.OnAmountChanged += value => moneyFromAction = value;

            foreach (var moneyToAdd in _moneyAmountsToAdd)
            {
                addedMoney += moneyToAdd;
                controller.AddAmount(moneyToAdd);
                Assert.AreEqual(addedMoney, moneyFromAction);
            }
        }

        [Test]
        public void Should_Controller_Notify_On_Subtracting()
        {
            BigDouble moneyFromAction = 0;
            BigDouble subtractedMoney = 0;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 0);

            foreach (var amountToAdd in _moneyAmountsToAdd)
            {
                subtractedMoney += amountToAdd;
                controller.AddAmount(amountToAdd);
            }

            controller.OnAmountChanged += value => moneyFromAction = value;

            foreach (var moneyToSubtract in _moneyAmountsToAdd)
            {
                subtractedMoney -= moneyToSubtract;
                controller.SubtractAmount(moneyToSubtract);
                Assert.AreEqual(subtractedMoney, moneyFromAction);
            }
        }

        [Test]
        public void Should_Controller_Remove_All_Listeners()
        {
            int callCountToIncrease = 0;
            int callCountToDecrease = 2;
            int expectedCallCount = 1;
            CurrencyType currencyType = CurrencyType.Money;

            var controller = new CurrencyController(currencyType, 0);
            controller.OnAmountChanged += _ => callCountToIncrease++;
            controller.OnAmountChanged += _ => callCountToDecrease--;

            controller.AddAmount(1);
            controller.RemoveAllListeners();

            Assert.AreEqual(expectedCallCount, callCountToIncrease);
            Assert.AreEqual(expectedCallCount, callCountToDecrease);
        }
    }
}
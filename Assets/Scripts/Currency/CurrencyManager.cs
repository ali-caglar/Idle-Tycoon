using System;
using System.Collections.Generic;
using BreakInfinity;
using Enums;
using Helpers;

namespace Currency
{
    public class CurrencyManager
    {
        private Dictionary<CurrencyType, CurrencyController> _currencyControllerDictionary;

        // public IReadOnlyDictionary<CurrencyType, BigDouble> CurrencyControllerDictionary => _currencyControllerDictionary;

        public CurrencyManager()
        {
            _currencyControllerDictionary = new Dictionary<CurrencyType, CurrencyController>
            {
                [CurrencyType.Money] = new(CurrencyType.Money, new BigDouble(Constants.MoneyStartValue)),
                [CurrencyType.Gem] = new(CurrencyType.Gem, new BigDouble(0)),
            };
        }

        public void AddAmount(CurrencyType currencyTypeToAdd, BigDouble amountToAdd)
        {
            var currencyController = GetCurrencyController(currencyTypeToAdd);
            currencyController.AddAmount(amountToAdd);
        }

        public void SubtractAmount(CurrencyType currencyTypeToSubtract, BigDouble amountToSubtract)
        {
            var currencyController = GetCurrencyController(currencyTypeToSubtract);
            currencyController.SubtractAmount(amountToSubtract);
        }

        public bool HasEnoughAmount(CurrencyType currencyTypeToCheck, BigDouble amountToCheck)
        {
            var currencyController = GetCurrencyController(currencyTypeToCheck);
            return currencyController.HasEnoughAmount(amountToCheck);
        }

        public void SubscribeToCurrencyChanges(CurrencyType currencyTypeToSubscribe, Action<BigDouble> onAmountChange)
        {
            var currencyController = GetCurrencyController(currencyTypeToSubscribe);
            currencyController.OnAmountChanged += onAmountChange;
        }

        public void UnSubscribeFromCurrencyChanges(CurrencyType currencyTypeToUnSubscribe, Action<BigDouble> onAmountChange)
        {
            var currencyController = GetCurrencyController(currencyTypeToUnSubscribe);
            currencyController.OnAmountChanged -= onAmountChange;
        }

        public void RemoveAllListeners()
        {
            foreach (var currencyController in _currencyControllerDictionary.Values)
            {
                currencyController.RemoveAllListeners();
            }
        }

        private CurrencyController GetCurrencyController(CurrencyType currencyType)
        {
            if (!_currencyControllerDictionary.ContainsKey(currencyType))
            {
                throw new Exception(
                    $"Currency Controller: {currencyType.ToString()} didn't initialized on the constructor");
            }

            return _currencyControllerDictionary[currencyType];
        }
    }
}
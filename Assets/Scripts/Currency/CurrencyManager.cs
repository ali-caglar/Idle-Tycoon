using System;
using System.Collections.Generic;
using BreakInfinity;
using Cysharp.Threading.Tasks;
using Datas.ScriptableDatas.Currencies;
using Enums;
using Zenject;

namespace Currency
{
    public class CurrencyManager
    {
        private readonly Dictionary<CurrencyType, CurrencyController> _currencyControllerDictionary;

        // public IReadOnlyDictionary<CurrencyType, BigDouble> CurrencyControllerDictionary => _currencyControllerDictionary;

        public CurrencyManager(SignalBus signalBus, List<CurrencyData> defaultCurrencies)
        {
            _currencyControllerDictionary = new Dictionary<CurrencyType, CurrencyController>();

            foreach (var currencyData in defaultCurrencies)
            {
                var currencyType = currencyData.UserData.currencyType;
                _currencyControllerDictionary[currencyType] = new CurrencyController(signalBus, currencyData);
            }
        }

        public BigDouble GetCurrentAmount(CurrencyType currencyType)
        {
            var currencyController = GetCurrencyController(currencyType);
            return currencyController.CurrentAmount;
        }

        public async UniTask AddAmount(CurrencyType currencyTypeToAdd, BigDouble amountToAdd)
        {
            var currencyController = GetCurrencyController(currencyTypeToAdd);
            await currencyController.AddAmount(amountToAdd);
        }

        public async UniTask SubtractAmount(CurrencyType currencyTypeToSubtract, BigDouble amountToSubtract)
        {
            var currencyController = GetCurrencyController(currencyTypeToSubtract);
            await currencyController.SubtractAmount(amountToSubtract);
        }

        public bool HasEnoughAmount(CurrencyType currencyTypeToCheck, BigDouble amountToCheck)
        {
            var currencyController = GetCurrencyController(currencyTypeToCheck);
            return currencyController.HasEnoughAmount(amountToCheck);
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
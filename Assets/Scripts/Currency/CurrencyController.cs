using BreakInfinity;
using Enums;
using Extensions;
using UnityEngine;
using Zenject;

namespace Currency
{
    public class CurrencyController
    {
        private CurrencyData _currencyData;
        private readonly SignalBus _signalBus;

        private CurrencyChangedSignal _currencyChangedSignal;

        public CurrencyType CurrencyType => _currencyData.currencyType;
        public BigDouble CurrentAmount => _currencyData.currentAmount;

        public CurrencyController(SignalBus signalBus, CurrencyData currencyData)
        {
            if (currencyData.currentAmount < 0)
            {
                currencyData.currentAmount = 0;
                Debug.LogWarning("Tried to initiate money with negative value.");
            }

            _signalBus = signalBus;
            _currencyData = currencyData;
        }

        public void AddAmount(BigDouble amountToAdd)
        {
            if (CheckForNegativeValue(amountToAdd))
            {
                Debug.LogWarning("Negative values are not handled.");
                return;
            }

            UpdateCurrencyAmount(amountToAdd);
            InvokeAmountChange();
        }

        public void SubtractAmount(BigDouble amountToSubtract)
        {
            if (CheckForNegativeValue(amountToSubtract))
            {
                Debug.LogWarning("Negative values are not handled.");
                return;
            }

            if (HasEnoughAmount(amountToSubtract))
            {
                UpdateCurrencyAmount(-amountToSubtract);
                InvokeAmountChange();
            }
        }

        public bool HasEnoughAmount(BigDouble amountToCheck)
        {
            return _currencyData.currentAmount >= amountToCheck;
        }

        private bool CheckForNegativeValue(BigDouble value)
        {
            return value < 0;
        }

        private void UpdateCurrencyAmount(BigDouble amountToAdd)
        {
            var currencyData = _currencyData;
            currencyData.currentAmount += amountToAdd;
            _currencyData = currencyData;
        }

        private void InvokeAmountChange()
        {
            _currencyChangedSignal ??= new CurrencyChangedSignal();
            _currencyChangedSignal.CurrencyData = this._currencyData;

            _signalBus.Fire(_currencyChangedSignal);
        }
    }
}
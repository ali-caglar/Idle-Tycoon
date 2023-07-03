using System;
using BreakInfinity;
using Enums;
using UnityEngine;

namespace Currency
{
    public class CurrencyController
    {
        public event Action<BigDouble> OnAmountChanged;

        public CurrencyType CurrencyType { get; private set; }
        public BigDouble CurrentAmount { get; private set; }

        public CurrencyController(CurrencyType currencyType, BigDouble currentAmount)
        {
            if (currentAmount < 0)
            {
                currentAmount = 0;
                Debug.LogWarning("Tried to initiate money with negative value.");
            }

            CurrencyType = currencyType;
            CurrentAmount = currentAmount;
        }

        public void AddAmount(BigDouble amountToAdd)
        {
            if (CheckForNegativeValue(amountToAdd))
            {
                Debug.LogWarning("Negative values are not handled.");
                return;
            }

            CurrentAmount += amountToAdd;
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
                CurrentAmount -= amountToSubtract;
                InvokeAmountChange();
            }
        }

        public bool HasEnoughAmount(BigDouble amountToCheck)
        {
            return CurrentAmount >= amountToCheck;
        }

        public void RemoveAllListeners()
        {
            if (OnAmountChanged == null) return;
            var listeners = OnAmountChanged.GetInvocationList();
            foreach (var listener in listeners)
            {
                OnAmountChanged -= listener as Action<BigDouble>;
            }
        }

        private bool CheckForNegativeValue(BigDouble value)
        {
            return value < 0;
        }

        private void InvokeAmountChange()
        {
            OnAmountChanged?.Invoke(CurrentAmount);
        }
    }
}
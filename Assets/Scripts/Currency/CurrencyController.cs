using System;
using BreakInfinity;
using Enums;

namespace Currency
{
    public class CurrencyController
    {
        public event Action<BigDouble> OnAmountChanged;

        public CurrencyType CurrencyType { get; private set; }
        public BigDouble CurrentAmount { get; private set; }

        public CurrencyController(CurrencyType currencyType, BigDouble currentAmount)
        {
            CurrencyType = currencyType;
            CurrentAmount = currentAmount;
        }

        public void AddAmount(BigDouble amountToAdd)
        {
            CurrentAmount.Add(amountToAdd);
            InvokeAmountChange();
        }

        public void SubtractAmount(BigDouble amountToSubtract)
        {
            if (HasEnoughAmount(amountToSubtract))
            {
                CurrentAmount.Subtract(amountToSubtract);
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

        private void InvokeAmountChange()
        {
            OnAmountChanged?.Invoke(CurrentAmount);
        }
    }
}
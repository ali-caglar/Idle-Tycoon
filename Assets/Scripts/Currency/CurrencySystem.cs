using System;
using BreakInfinity;
using Enums;
using UnityEngine;

namespace Currency
{
    public class CurrencySystem : MonoBehaviour
    {
        private CurrencyManager _currencyManager;

        private void Awake()
        {
            _currencyManager = new CurrencyManager();
        }

        private void OnDestroy()
        {
            _currencyManager.RemoveAllListeners();
        }

        public void AddAmount(CurrencyType currencyTypeToAdd, BigDouble amountToAdd)
        {
            _currencyManager.AddAmount(currencyTypeToAdd, amountToAdd);
        }

        public void SubtractAmount(CurrencyType currencyTypeToSubtract, BigDouble amountToSubtract)
        {
            _currencyManager.SubtractAmount(currencyTypeToSubtract, amountToSubtract);
        }

        public bool HasEnoughAmount(CurrencyType currencyTypeToCheck, BigDouble amountToCheck)
        {
            return _currencyManager.HasEnoughAmount(currencyTypeToCheck, amountToCheck);
        }

        public void SubscribeToCurrencyChanges(CurrencyType currencyTypeToSubscribe, Action<BigDouble> onAmountChange)
        {
            _currencyManager.SubscribeToCurrencyChanges(currencyTypeToSubscribe, onAmountChange);
        }

        public void UnSubscribeFromCurrencyChanges(CurrencyType currencyTypeToUnSubscribe, Action<BigDouble> onAmountChange)
        {
            _currencyManager.UnSubscribeFromCurrencyChanges(currencyTypeToUnSubscribe, onAmountChange);
        }
    }
}
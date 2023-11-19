using BreakInfinity;
using Cysharp.Threading.Tasks;
using Datas.ScriptableDatas.Currencies;
using Enums;
using UnityEngine;
using Zenject;

namespace Currency
{
    public class CurrencyController
    {
        private readonly CurrencyData _currencyData;
        private readonly SignalBus _signalBus;

        private CurrencyChangedSignal _currencyChangedSignal;

        public CurrencyType CurrencyType => _currencyData.UserData.currencyType;
        public BigDouble CurrentAmount => _currencyData.UserData.currentAmount;

        public CurrencyController(SignalBus signalBus, CurrencyData currencyData)
        {
            if (currencyData.UserData.currentAmount < 0)
            {
                currencyData.UserData.currentAmount = 0;
                Utility.Logger.Log(LogType.Warning, "Tried to initiate money with negative value.");
            }

            _signalBus = signalBus;
            _currencyData = currencyData;
        }

        public async UniTask AddAmount(BigDouble amountToAdd)
        {
            if (CheckForNegativeValue(amountToAdd))
            {
                Utility.Logger.Log(LogType.Warning, "Negative values are not handled.");
                return;
            }

            await UpdateCurrencyAmount(amountToAdd);
            InvokeAmountChange();
        }

        public async UniTask SubtractAmount(BigDouble amountToSubtract)
        {
            if (CheckForNegativeValue(amountToSubtract))
            {
                Utility.Logger.Log(LogType.Warning, "Negative values are not handled.");
                return;
            }

            if (HasEnoughAmount(amountToSubtract))
            {
                await UpdateCurrencyAmount(-amountToSubtract);
                InvokeAmountChange();
            }
        }

        public bool HasEnoughAmount(BigDouble amountToCheck)
        {
            return _currencyData.UserData.currentAmount >= amountToCheck;
        }

        private bool CheckForNegativeValue(BigDouble value)
        {
            return value < 0;
        }

        private async UniTask UpdateCurrencyAmount(BigDouble amountToAdd)
        {
            _currencyData.UserData.currentAmount += amountToAdd;
            await _currencyData.SaveUserData();
        }

        private void InvokeAmountChange()
        {
            _currencyChangedSignal ??= new CurrencyChangedSignal();
            _currencyChangedSignal.CurrencyDataModel = _currencyData.UserData;

            _signalBus.Fire(_currencyChangedSignal);
        }
    }
}
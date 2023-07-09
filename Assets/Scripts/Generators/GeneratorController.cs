using System;
using BreakInfinity;
using Currency;
using Datas.DataModels.Generators;
using Datas.ScriptableDatas.Generators;
using Enums;
using Helpers;
using TimeTick;
using UnityEngine;

namespace Generators
{
    public class GeneratorController : MonoBehaviour
    {
        [SerializeField] private GeneratorData generatorData;

        private TimeTickSystem _tickSystem;

        #region Private Getters From Generator Data

        private ulong CurrentLevel => generatorData.UserData.CurrentLevel;
        private float InitialTickDuration => generatorData.DataModel.profitDataModel.durationToEarnProfit;
        private BigDouble ProfitPerLevel => generatorData.DataModel.profitDataModel.profitPerLevel;

        #endregion

        public void Initialize(TimeTickSystem tickSystem)
        {
            _tickSystem = tickSystem;
            InitController();
        }

        public BigDouble GetProductionPerTick()
        {
            return ProfitPerLevel * CurrentLevel;
        }

        public BigDouble GetProductionPerSecond()
        #region PUBLIC METHODS

        public void UnlockGenerator()
        {
            if (UserData.IsUnlocked) return;

            var unlockCost = GetUnlockCost();
            if (_currencySystem.HasEnoughAmount(CostType, unlockCost))
            {
                UserData.IsUnlocked = true;
                generatorData.Save();
                _currencySystem.SubtractAmount(CostType, unlockCost);
                OnGeneratorBought?.Invoke();
            }
        }

        public void BuyGenerator()
        {
            if (!UserData.IsUnlocked) return;

            var unlockCost = GetNextCost();
            if (_currencySystem.HasEnoughAmount(CostType, unlockCost))
            {
                UserData.CurrentLevel++;
                generatorData.Save();
                _currencySystem.SubtractAmount(CostType, unlockCost);
                OnGeneratorBought?.Invoke();
            }
        }

        #endregion
    }
}
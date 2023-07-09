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
        #region EVENTS

        public event Action OnGeneratorBought;

        #endregion

        #region SERIALIZED PRIVATE FIELDS

        [SerializeField] private GeneratorData generatorData;

        #endregion

        #region PRIVATE FIELDS

        private TimeTickController _timeTickController;

        // Dependencies
        private GeneratorManager _generatorManager;
        private TimeTickSystem _timeSystem;
        private CurrencySystem _currencySystem;

        #endregion

        #region PUBLIC PROPERTIES

        public BigDouble ProductionPerTick => GetProductionPerTick();
        public BigDouble ProductionPerSecond => GetProductionPerSecond();
        public BigDouble UnlockCost => GetUnlockCost();
        public BigDouble NextCost => GetNextCost();

        #endregion

        #region PRIVATE PROPERTIES

        // Getters from GeneratorManager
        private float ProfitMultiplierFromBonus => 1;

        // Getters from generator data
        private CurrencyType CostType => generatorData.CostType;
        private CurrencyType ProductionType => generatorData.ProductionType;
        private ulong CurrentLevel => generatorData.UserData.CurrentLevel;
        private UserDataForGeneratorDataModel UserData => generatorData.UserData;
        private GeneratorCostDataModel CostData => generatorData.DataModel.costDataModel;
        private GeneratorProfitDataModel ProfitData => generatorData.DataModel.profitDataModel;

        #endregion

        #region INITIALIZER

        public void Initialize(GeneratorManager generatorManager, TimeTickSystem timeSystem,
            CurrencySystem currencySystem)
        {
            _generatorManager = generatorManager;
            _timeSystem = timeSystem;
            _currencySystem = currencySystem;
            InitController();
        }

        private void InitController()
        {
            if (UserData.IsUnlocked)
            {
                _timeTickController = new TimeTickController(0, ProfitData.durationToEarnProfit);
                _timeTickController.OnTimeTick += AddProductionToCurrencyController;

                _timeSystem.AddNewTimeTick(_timeTickController);
            }
        }

        #endregion

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
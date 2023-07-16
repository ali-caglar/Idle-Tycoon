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
    [RequireComponent(typeof(WorkerController))]
    public class GeneratorController : MonoBehaviour
    {
        #region EVENTS

        /// <summary>
        /// This event invoked whether generator is unlocked or bought another.
        /// </summary>
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
        private WorkerController _workerController;

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
        private CurrencyType CostType => generatorData.DataModel.costDataModel.costCurrencyType;
        private CurrencyType ProductionType => generatorData.DataModel.profitDataModel.profitCurrencyType;
        private ulong CurrentLevel => generatorData.UserData.CurrentLevel;
        private UserDataForGeneratorDataModel UserData => generatorData.UserData;
        private GeneratorCostDataModel CostData => generatorData.DataModel.costDataModel;
        private GeneratorProfitDataModel ProfitData => generatorData.DataModel.profitDataModel;

        // Getters from worker controller
        private bool IsAutomated => _workerController.HasWorker;

        #endregion

        #region INITIALIZER

        public void Initialize(GeneratorManager generatorManager, TimeTickSystem timeSystem,
            CurrencySystem currencySystem)
        {
            _generatorManager = generatorManager;
            _timeSystem = timeSystem;
            _currencySystem = currencySystem;
            _workerController = GetComponent<WorkerController>();

            InitSubControllers();
            InitController();
        }

        private void InitSubControllers()
        {
            WorkerData workerData = _generatorManager.GetWorkerData(generatorData.UserData.AssignedWorkerIdentifierID);
            _workerController.Initialize(generatorData.DataModel.identifierID, workerData);
        }

        private void InitController()
        {
            if (UserData.IsUnlocked)
            {
                _timeTickController = new TimeTickController(0, GetTickDuration(), IsAutomated);
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

        public void AssignWorker(string workerID)
        {
            WorkerData workerData = _generatorManager.GetWorkerData(workerID);
            if (_workerController.AssignWorker(workerData))
            {
                UserData.AssignedWorkerIdentifierID = workerID;
            }

            UpdateTickAutomation();
        }

        public void RemoveWorker()
        {
            _workerController.RemoveWorker();
            UserData.AssignedWorkerIdentifierID = string.Empty;
            UpdateTickAutomation();
        }

        #endregion

        #region PRIVATE METHODS

        private void AddProductionToCurrencyController()
        {
            var amountToAdd = GetProductionPerTick();
            _currencySystem.AddAmount(ProductionType, amountToAdd);
        }

        private BigDouble GetProductionPerTick()
        {
            var production = (CurrentLevel * ProfitMultiplierFromBonus) * ProfitData.profitPerLevel;
            return production * _workerController.ProfitMultiplier;
        }

        private BigDouble GetProductionPerSecond()
        {
            return GetProductionPerTick() / GetTickDuration();
        }

        private BigDouble GetUnlockCost()
        {
            return CostData.baseCost;
        }

        private BigDouble GetNextCost()
        {
            var buyOption = _generatorManager.BuyOption;
            var currentMoney = _currencySystem.GetCurrentAmount(CostType);
            var amountToBuy = Calculator.CalculateAmountToBuy(buyOption, CostData, CurrentLevel, currentMoney);
            var price = Calculator.GetGeneratorCost(CostData, amountToBuy, CurrentLevel);
            return price * _workerController.CostDiscount;
        }

        private float GetTickDuration()
        {
            return ProfitData.durationToEarnProfit * _workerController.TimeMultiplier;
        }

        private void UpdateTickAutomation()
        {
            if (_timeTickController is { TimeIdentifier: TimeTickIdentifier.Custom })
            {
                _timeTickController.SetAutomation(IsAutomated);
            }
        }

        #endregion
    }
}
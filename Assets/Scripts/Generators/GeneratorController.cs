using System;
using BreakInfinity;
using Currency;
using Datas.DataModels.Generators;
using Datas.ScriptableDatas.Generators;
using Enums;
using Helpers;
using TimeTick;
using UnityEngine;
using Zenject;

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
        private TimeTickManager _timeManager;
        private CurrencyManager _currencyManager;
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

        #region CONSTRUCTOR

        [Inject]
        private void Construct(GeneratorManager generatorManager, WorkerManager workerManager, TimeTickManager timeSystem, CurrencyManager currencySystem)
        {
            _generatorManager = generatorManager;
            _workerManager = workerManager;
            _timeManager = timeSystem;
            _currencyManager = currencySystem;
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
            if (!UserData.IsUnlocked) return;
            TimeTickControllerData timeData = new TimeTickControllerData
            {
                isAutomated = IsAutomated,
                tickTimer = 0,
                tickDuration = GetTickDuration(),
                timeIdentifier = TimeTickIdentifier.Custom
            };

            _timeTickController = new TimeTickController(timeData);
            _timeTickController.OnTimeTick += AddProductionToCurrencyController;

            _timeManager.AddNewCustomTickController(_timeTickController);
        }

        #endregion

        #region PUBLIC METHODS

        public void UnlockGenerator()
        {
            if (UserData.IsUnlocked) return;

            var unlockCost = GetUnlockCost();
            if (_currencyManager.HasEnoughAmount(CostType, unlockCost))
            {
                UserData.IsUnlocked = true;
                generatorData.Save();
                _currencyManager.SubtractAmount(CostType, unlockCost);
                OnGeneratorBought?.Invoke();
            }
        }

        public void BuyGenerator()
        {
            if (!UserData.IsUnlocked) return;

            var unlockCost = GetNextCost();
            if (_currencyManager.HasEnoughAmount(CostType, unlockCost))
            {
                UserData.CurrentLevel++;
                generatorData.Save();
                _currencyManager.SubtractAmount(CostType, unlockCost);
                OnGeneratorBought?.Invoke();
            }
        }

        public void CollectProductionOnTap()
        {
            if (IsAutomated || _timeTickController is { GetProgress: < 1 }) return;
            AddProductionToCurrencyController();
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
            _currencyManager.AddAmount(ProductionType, amountToAdd);
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
            var currentMoney = _currencyManager.GetCurrentAmount(CostType);
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
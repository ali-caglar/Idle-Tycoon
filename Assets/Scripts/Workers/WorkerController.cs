using System;
using Datas.ScriptableDatas.Generators;
using Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Workers
{
    public class WorkerController : MonoBehaviour
    {
        #region PRIVATE FIELDS

        private string _generatorID;

        #endregion

        #region PUBLIC PROPERTIES

        [CanBeNull] public WorkerData Worker { get; private set; }

        public bool HasWorker => Worker != null;

        public float ProfitMultiplier => GetBonusMultiplier();

        public float CostDiscount => GetBonusMultiplier();

        public float TimeMultiplier => GetBonusMultiplier();

        #endregion

        #region PRIVATE PROPERTIES

        private WorkerBonusType BonusType =>
            Worker == null ? WorkerBonusType.None : Worker.DataModel.bonusDataModel.bonusType;

        #endregion

        #region INITIALIZER

        public void Initialize(string generatorID, WorkerData workerData)
        {
            if (string.IsNullOrEmpty(generatorID))
            {
                throw new Exception("Generator ID can't be null or empty.");
            }

            _generatorID = generatorID;
            AssignWorker(workerData);
        }

        #endregion

        #region PUBLIC METHODS

        public bool AssignWorker(WorkerData workerData)
        {
            if (workerData == null) return false;

            bool isUnlocked = workerData.UserData.IsUnlocked;
            if (isUnlocked)
            {
                Worker = workerData;
                Worker.UserData.BelongedGeneratorIdentifierID = _generatorID;
            }

            return isUnlocked;
        }

        public void RemoveWorker()
        {
            if (Worker != null)
            {
                Worker.UserData.BelongedGeneratorIdentifierID = string.Empty;
            }

            Worker = null;
        }

        #endregion

        #region PRIVATE METHODS

        private float GetBonusMultiplier()
        {
            if (BonusType == WorkerBonusType.None || Worker == null)
            {
                return 1;
            }

            WorkerBonusType bonusType = Worker.DataModel.bonusDataModel.bonusType;
            float bonusPercentage = Worker.DataModel.bonusDataModel.bonusPercentage / 100;
            float multiplier = bonusType switch
            {
                WorkerBonusType.None => 1,
                WorkerBonusType.Profit => 1 + bonusPercentage,
                WorkerBonusType.Time => 1 - bonusPercentage,
                WorkerBonusType.Discount => 1 - bonusPercentage,
                _ => throw new ArgumentOutOfRangeException(nameof(bonusType), bonusType, null)
            };

            return multiplier;
        }

        #endregion
    }
}
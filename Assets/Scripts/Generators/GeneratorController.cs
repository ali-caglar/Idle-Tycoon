using BreakInfinity;
using Datas.ScriptableDatas.Generators;
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
        private float TickDuration => generatorData.DataModel.profitDataModel.durationToEarnProfit;
        private BigDouble ProfitPerLevel => generatorData.DataModel.profitDataModel.profitPerLevel;

        #endregion

        private void Awake()
        {
            
        }

        public BigDouble GetProductionPerTick()
        {
            return ProfitPerLevel * CurrentLevel;
        }

        public BigDouble GetProductionPerSecond()
        {
            return GetProductionPerTick() / TickDuration;
        }
    }
}
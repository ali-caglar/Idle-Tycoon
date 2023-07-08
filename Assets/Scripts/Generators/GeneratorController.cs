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
        {
            return GetProductionPerTick() / InitialTickDuration;
        }

        private void InitController()
        {
            if (generatorData.UserData.IsUnlocked)
            {
                var timeTickController = new TimeTickController(0, InitialTickDuration);
                _tickSystem.AddNewTimeTick(timeTickController);
            }
        }
    }
}
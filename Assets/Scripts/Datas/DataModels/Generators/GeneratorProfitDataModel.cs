using BreakInfinity;
using Enums;

namespace Datas.DataModels.Generators
{
    [System.Serializable]
    public struct GeneratorProfitDataModel
    {
        public CurrencyType profitCurrencyType;
        public BigDouble profitPerLevel;
        public float durationToEarnProfit;
    }
}
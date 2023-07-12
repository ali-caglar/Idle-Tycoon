using BreakInfinity;
using Enums;

namespace Datas.DataModels.Generators
{
    [System.Serializable]
    public struct GeneratorCostDataModel
    {
        public CurrencyType costCurrencyType;
        public BigDouble baseCost;
        public float costCoefficient;
    }
}
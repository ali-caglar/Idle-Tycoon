using BreakInfinity;
using Enums;

namespace Datas.DataModels.Workers
{
    [System.Serializable]
    public struct WorkerCostDataModel
    {
        public CurrencyType costCurrencyType;
        public BigDouble unlockCost;
    }
}
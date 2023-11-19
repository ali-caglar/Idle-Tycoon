using BreakInfinity;
using Enums;
using Save;

namespace Datas.DataModels.Currencies
{
    [System.Serializable]
    public class CurrencyDataModel : BaseDataModel<CurrencyDataModel>
    {
        public CurrencyType currencyType;
        public BigDouble currentAmount;
    }
}
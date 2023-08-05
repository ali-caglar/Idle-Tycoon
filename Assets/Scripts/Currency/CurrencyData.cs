using BreakInfinity;
using Enums;

namespace Currency
{
    [System.Serializable]
    public struct CurrencyData
    {
        public CurrencyType currencyType;
        public BigDouble currentAmount;
    }
}
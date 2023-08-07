using System.Collections.Generic;
using UnityEngine;

namespace Currency
{
    [CreateAssetMenu(fileName = "Default Currencies Data", menuName = "Data/Currency/Default Data", order = 0)]
    public class DefaultCurrenciesData : ScriptableObject
    {
        [SerializeField] private List<CurrencyData> defaultDatas;

        public List<CurrencyData> DefaultCurrencies => defaultDatas;
    }
}
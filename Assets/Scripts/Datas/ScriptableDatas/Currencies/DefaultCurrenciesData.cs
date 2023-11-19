using System.Collections.Generic;
using Datas.DataModels.Currencies;
using UnityEngine;

namespace Datas.ScriptableDatas.Currencies
{
    [CreateAssetMenu(fileName = "Default Currencies Data", menuName = "Data/Currency/Default Currency List Data", order = 0)]
    public class DefaultCurrenciesData : ScriptableObject
    {
        [SerializeField] private List<CurrencyData> defaultDatas;

        public List<CurrencyData> DefaultCurrencies => defaultDatas;
    }
}
using Datas.DataModels.Currencies;
using Save.ScriptableObjects;
using UnityEngine;

namespace Datas.ScriptableDatas.Currencies
{
    [CreateAssetMenu(fileName = "Currency Data", menuName = "Data/Currency/Currency Data", order = 0)]
    public class CurrencyData : ScriptableObjectWithSaveAndLoadAsync<CurrencyDataModel, CurrencyDataModel>
    {
        
    }
}
using Save.ScriptableObjects;
using UnityEngine;

namespace Save.Demo.Scripts
{
    [CreateAssetMenu(fileName = "temp", menuName = "temp2", order = 0)]
    public class DemoSaveDataWithUserData : ScriptableObjectWithSaveAndLoad<DemoSaveFoo, DemoSaveBar>
    {
        
    }

    [System.Serializable]
    public class DemoSaveFoo : BaseDataModel<DemoSaveFoo>
    {
        public string birthPlace;
        public uint age;
        public double initialMoney;
    }

    [System.Serializable]
    public class DemoSaveBar : BaseDataModel<DemoSaveBar>
    {
        public string userName;
        public double currentMoney;
    }
}
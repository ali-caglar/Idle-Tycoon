using Save.ScriptableObjects;
using UnityEngine;

namespace Save.Demo.Scripts
{
    [CreateAssetMenu(fileName = "temp", menuName = "temp1", order = 0)]
    public class DemoSaveData : ScriptableObjectWithSaveAndLoad<DemoSaveFooBar>
    {
        
    }

    [System.Serializable]
    public class DemoSaveFooBar : BaseDataModel<DemoSaveFooBar>
    {
        public int year;
        public string alias;
        public bool isWorking;
        public float timer;

        public DemoSaveFooBaro baro;
    }

    [System.Serializable]
    public class DemoSaveFooBaro
    {
        public double unlockPrice;
    }
}
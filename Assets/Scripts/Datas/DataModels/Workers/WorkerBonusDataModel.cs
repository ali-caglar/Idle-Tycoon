using Enums;
using Sirenix.OdinInspector;

namespace Datas.DataModels.Workers
{
    [System.Serializable]
    public struct WorkerBonusDataModel
    {
        public WorkerBonusType bonusType;
        [PropertyRange(0, 100)] public float bonusPercentage;
    }
}
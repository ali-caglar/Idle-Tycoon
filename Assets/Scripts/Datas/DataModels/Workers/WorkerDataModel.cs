namespace Datas.DataModels.Workers
{
    [System.Serializable]
    public class WorkerDataModel
    {
        public string identifierID;
        public string displayName;
        public WorkerCostDataModel costDataModel;
        public WorkerBonusDataModel bonusDataModel;
    }
}
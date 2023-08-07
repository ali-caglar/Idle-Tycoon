using System.Collections.Generic;
using Datas.ScriptableDatas.Generators;
using Zenject;

namespace Workers
{
    public class WorkerManager
    {
        #region PRIVATE FIELDS

        private Dictionary<string, WorkerData> _workersDictionary;

        #endregion

        #region CONSTRUCTOR

        [Inject]
        private void Construct(List<WorkerData> workerDatas)
        {
            _workersDictionary = new Dictionary<string, WorkerData>();
            foreach (var workerData in workerDatas)
            {
                _workersDictionary[workerData.DataModel.identifierID] = workerData;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public WorkerData GetWorkerData(string workerID)
        {
            return _workersDictionary.TryGetValue(workerID, out var workerData)
                ? workerData
                : null;
        }

        #endregion
    }
}
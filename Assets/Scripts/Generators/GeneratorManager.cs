using System.Linq;
using Datas.ScriptableDatas.Generators;
using Enums;
using UnityEngine;

namespace Generators
{
    public class GeneratorManager : MonoBehaviour
    {
        [SerializeField] private WorkerData[] workerDatas;

        public MultiBuyOption BuyOption { get; private set; }

        public WorkerData GetWorkerData(string workerID)
        {
            return workerDatas.FirstOrDefault(x => x.DataModel.identifierID == workerID);
        }
    }
}
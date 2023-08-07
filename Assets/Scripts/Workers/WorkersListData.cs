using System.Collections.Generic;
using Datas.ScriptableDatas.Generators;
using UnityEngine;

namespace Workers
{
    [CreateAssetMenu(fileName = "Workers List Data", menuName = "Data/Workers/Workers List", order = 0)]
    public class WorkersListData : ScriptableObject
    {
        [SerializeField] private List<WorkerData> workerDatas;

        public List<WorkerData> WorkersList => workerDatas;
    }
}
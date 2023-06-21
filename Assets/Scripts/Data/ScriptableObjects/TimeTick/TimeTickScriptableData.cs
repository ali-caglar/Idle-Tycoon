using System.Linq;
using Data.DataModels.TimeTick;
using Enums;
using UnityEngine;

namespace Data.ScriptableObjects.TimeTick
{
    [CreateAssetMenu(fileName = "Time Tick Data", menuName = "Data/Systems/Time Tick", order = 0)]
    public class TimeTickScriptableData : ScriptableObject
    {
        [SerializeField] private TimeTickData[] timeTickData;

        public bool GetTickData(TimeTickIdentifier timeIdentifier, out TimeTickData tickData)
        {
            tickData = timeTickData.FirstOrDefault(x => x.TimeIdentifier == timeIdentifier);
            return tickData != null;
        }

        public void RemoveAllListeners()
        {
            foreach (var tickData in timeTickData)
            {
                tickData.RemoveAllListeners();
            }
        }
    }
}
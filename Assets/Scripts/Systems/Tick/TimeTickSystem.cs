using System;
using Data.DataModels.TimeTick;
using Data.ScriptableObjects.TimeTick;
using Enums;
using UnityEngine;

namespace Systems.Tick
{
    public class TimeTickSystem : MonoBehaviour
    {
        [SerializeField] private TimeTickScriptableData timeTickScriptableData;

        private void OnDestroy()
        {
            timeTickScriptableData.RemoveAllListeners();
        }

        public void SubscribeToTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeTickScriptableData.GetTickData(timeIdentifier, out TimeTickData tickData))
            {
                tickData.OnTimeTick += timeTickHandler;
            }
        }

        public void UnSubscribeFromTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeTickScriptableData.GetTickData(timeIdentifier, out TimeTickData tickData))
            {
                tickData.OnTimeTick -= timeTickHandler;
            }
        }
    }
}
using System;
using Enums;
using UnityEngine;

namespace Data.DataModels.TimeTick
{
    [Serializable]
    public class TimeTickData
    {
        public event Action OnTimeTick;

        [SerializeField] public TimeTickIdentifier timeIdentifier;
        [SerializeField] private float tickDuration;

        private float _tickTimer;

        public TimeTickIdentifier TimeIdentifier => timeIdentifier;

        public void UpdateTimer(float timeToIncrease)
        {
            _tickTimer += timeToIncrease;
            if (_tickTimer >= tickDuration)
            {
                _tickTimer -= tickDuration;
                InvokeTimeTick();
            }
        }

        public void RemoveAllListeners()
        {
            if (OnTimeTick == null) return;
            var listeners = OnTimeTick.GetInvocationList();
            foreach (var listener in listeners)
            {
                OnTimeTick -= listener as Action;
            }
        }

        private void InvokeTimeTick()
        {
            OnTimeTick?.Invoke();
        }
    }
}
using System;
using Enums;

namespace TimeTick
{
    public class TimeTickController
    {
        public event Action OnTimeTick;

        private float _tickTimer;
        private float _tickDuration;
        public TimeTickIdentifier TimeIdentifier { get; }

        public float TickTimer => _tickTimer;
        public float TickDuration => _tickDuration;
        public float GetProgress => _tickTimer / _tickDuration;

        public TimeTickController(TimeTickIdentifier timeIdentifier, float startTime, float totalTime)
        {
            if (totalTime <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }
        
            _tickTimer = startTime;
            _tickDuration = totalTime;
            TimeIdentifier = timeIdentifier;
        }

        public void UpdateTimer(float timeToIncrease)
        {
            _tickTimer += timeToIncrease;
            if (_tickTimer >= _tickDuration)
            {
                _tickTimer -= _tickDuration;
                InvokeTimeTick();
            }
        }

        public void UpdateTickDuration(float newTickDuration)
        {
            _tickDuration = newTickDuration;
            if (_tickTimer >= _tickDuration)
            {
                _tickTimer = 0;
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
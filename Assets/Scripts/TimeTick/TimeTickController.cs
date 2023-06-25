using System;
using Enums;

namespace TimeTick
{
    public class TimeTickController
    {
        public event Action OnTimeTick;

        public float TickTimer { get; private set; }
        public float TickDuration { get; private set; }
        public TimeTickIdentifier TimeIdentifier { get; }

        public float GetProgress => TickTimer / TickDuration;

        public TimeTickController(TimeTickIdentifier timeIdentifier, float startTime, float totalTime)
        {
            if (totalTime <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }

            TickTimer = startTime;
            TickDuration = totalTime;
            TimeIdentifier = timeIdentifier;
        }

        public void UpdateTimer(float timeToIncrease)
        {
            TickTimer += timeToIncrease;
            if (TickTimer >= TickDuration)
            {
                TickTimer -= TickDuration;
                InvokeTimeTick();
            }
        }

        public void UpdateTickDuration(float newTickDuration)
        {
            TickDuration = newTickDuration;
            if (TickTimer >= TickDuration)
            {
                TickTimer = 0;
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
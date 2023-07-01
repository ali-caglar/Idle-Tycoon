using System;
using Enums;
using UnityEngine;

namespace TimeTick
{
    public class TimeTickController
    {
        public event Action OnTimeTick;

        public float TickTimer { get; private set; }
        public float TickDuration { get; private set; }
        public TimeTickIdentifier TimeIdentifier { get; }

        public float GetProgress => TickTimer / TickDuration;

        public TimeTickController(float startTime, float totalTime, TimeTickIdentifier timeIdentifier = TimeTickIdentifier.Custom)
        {
            if (startTime < 0)
            {
                startTime = 0;
            }

            if (totalTime <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }

            TickTimer = startTime % totalTime;
            TickDuration = totalTime;
            TimeIdentifier = timeIdentifier;

            if (startTime >= totalTime)
            {
                Debug.LogWarning("Start time was greater or equal to total duration, " +
                                 "start time updated upon total timer and there was no event invocation.");
            }
        }

        public void UpdateTimer(float timeToIncrease)
        {
            if (timeToIncrease <= 0)
            {
                Debug.LogWarning("This method should only increase the timer, subtracting not supported.");
                return;
            }

            TickTimer += timeToIncrease;
            while (TickTimer >= TickDuration)
            {
                TickTimer -= TickDuration;
                InvokeTimeTick();
            }
        }

        public void UpdateTickDuration(float newTickDuration)
        {
            if (newTickDuration <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }

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
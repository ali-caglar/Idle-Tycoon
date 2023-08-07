using System;
using Enums;
using Extensions;
using UnityEngine;

namespace TimeTick
{
    [Serializable]
    public class TimeTickController
    {
        #region EVENTS

        /// <summary>
        /// Invoked when timer got reset after it exceeds the duration.
        /// </summary>
        public event Action OnTimeTick;

        #endregion

        #region PRIVATE FIELDS

        private TimeTickControllerData _tickControllerData;

        #endregion

        #region PROPERTIES

        public bool IsAutomated { get; private set; }
        public float TickTimer { get; private set; }
        public float TickDuration { get; private set; }
        public TimeTickIdentifier TimeIdentifier { get; }

        /// <summary>
        /// Return progression from 0 to 1.
        /// </summary>
        public float GetProgress => TickTimer / TickDuration;

        #endregion

        #region CONSTRUCTOR

        /// <exception cref="Exception">Not handling negative values</exception>
        public TimeTickController(TimeTickControllerData timeData)
        {
            _tickControllerData = timeData;

            if (_tickControllerData.tickTimer < 0)
            {
                _tickControllerData.tickTimer = 0;
            }

            if (timeData.tickDuration <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }

            if (_tickControllerData.timeIdentifier != TimeTickIdentifier.Custom)
            {
                _tickControllerData.isAutomated = true;
            }

            IsAutomated = _tickControllerData.isAutomated;
            TickTimer = _tickControllerData.tickTimer % _tickControllerData.tickDuration;
            TickDuration = _tickControllerData.tickDuration;
            TimeIdentifier = _tickControllerData.timeIdentifier;

            if (timeData.tickTimer >= timeData.tickDuration)
            {
                Debug.LogWarning("Start time was greater or equal to total duration, " +
                                 "start time updated upon total timer and there was no event invocation.");
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Increases the timer by given parameter.
        /// If controller is automated after the increase; if timer is exceeds the duration it'll reset
        /// the timer and invoke the time tick event.
        /// </summary>
        /// <param name="timeToIncrease">Increase by value</param>
        public void UpdateTimer(float timeToIncrease)
        {
            if (timeToIncrease <= 0)
            {
                Debug.LogWarning("This method should only increase the timer, subtracting not supported.");
                return;
            }

            if (TickTimer < TickDuration)
            {
                TickTimer += timeToIncrease;
            }

            while (IsAutomated && IsTimerExceededDuration())
            {
                TickTimer -= TickDuration;
                InvokeTimeTick();
            }
        }

        /// <summary>
        /// Updates the total duration of the timer.
        /// If controller is automated after the update; if timer is exceeds the new duration it'll reset
        /// the timer and invoke the time tick event.
        /// </summary>
        /// <param name="newTickDuration">New total duration.</param>
        /// <exception cref="Exception">Can't be less or equal to 0.</exception>
        public void UpdateTickDuration(float newTickDuration)
        {
            if (newTickDuration <= 0)
            {
                throw new Exception("Tick Duration can't be less than or equal to 0");
            }

            TickDuration = newTickDuration;
            if (IsAutomated && IsTimerExceededDuration())
            {
                TickTimer = 0;
                InvokeTimeTick();
            }
        }

        /// <summary>
        /// It'll reset the timer and invoke the time tick event so it can start again.
        /// </summary>
        public void RenewTimer()
        {
            if (!IsAutomated && IsTimerExceededDuration())
            {
                TickTimer -= TickDuration;
                InvokeTimeTick();
            }
        }

        /// <summary>
        /// Updates automation. If set to automated controller handles renewing timer by itself.
        /// Pre-defined controllers always automated and can't be changed. 
        /// </summary>
        /// <param name="isAutomated">Set automation by value</param>
        public void SetAutomation(bool isAutomated)
        {
            IsAutomated = TimeIdentifier != TimeTickIdentifier.Custom || isAutomated;
        }

        /// <summary>
        /// Removes all the event listener.
        /// </summary>
        public void RemoveAllListeners()
        {
            if (OnTimeTick == null) return;
            var listeners = OnTimeTick.GetInvocationList();
            foreach (var listener in listeners)
            {
                OnTimeTick -= listener as Action;
            }
        }

        #endregion

        #region PRIVATE METHODS

        private bool IsTimerExceededDuration()
        {
            return TickTimer.IsExceeded(TickDuration);
        }

        private void InvokeTimeTick()
        {
            OnTimeTick?.Invoke();
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Enums;
using UnityEngine;
using Zenject;

namespace TimeTick
{
    public class TimeTickManager : ITickable
    {
        #region PRIVATE FIELDS

        private List<TimeTickController> _timeTickControllers;

        #endregion

        #region PROPERTIES

        public ReadOnlyCollection<TimeTickController> TimeTickControllers => _timeTickControllers.AsReadOnly();

        #endregion

        #region CONSTRUCTOR

        public TimeTickManager(List<TimeTickControllerData> defaultControllers)
        {
            _timeTickControllers = new List<TimeTickController>();
            foreach (var controllerData in defaultControllers)
            {
                _timeTickControllers.Add(new TimeTickController(controllerData));
            }
        }

        public void Tick()
        {
            UpdateTimers(Time.deltaTime);
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Adds new custom tick controller.
        /// Can't add the same custom controller or new controller with a pre-defined identifier.
        /// </summary>
        /// <param name="timeTickController">Controller to add</param>
        public void AddNewCustomTickController(TimeTickController timeTickController)
        {
            if (timeTickController.TimeIdentifier != TimeTickIdentifier.Custom)
            {
                Debug.LogWarning("Other identifiers than 'custom' are already added.");
                return;
            }

            _timeTickControllers ??= new List<TimeTickController>();
            if (!_timeTickControllers.Contains(timeTickController))
            {
                _timeTickControllers.Add(timeTickController);
            }
            else
            {
                Debug.LogWarning("Tried to add that is already existed in the list.");
            }
        }

        /// <summary>
        /// Removes custom tick controller which is added already.
        /// Can't remove the pre-defined controllers.
        /// </summary>
        /// <param name="timeTickController">Controller to remove</param>
        public void RemoveCustomTickController(TimeTickController timeTickController)
        {
            if (_timeTickControllers == null || _timeTickControllers.Count == 0) return;
            if (timeTickController.TimeIdentifier != TimeTickIdentifier.Custom)
            {
                Debug.LogWarning("You shouldn't remove the default tick controllers.");
                return;
            }

            if (_timeTickControllers.Contains(timeTickController))
            {
                timeTickController.RemoveAllListeners();
                _timeTickControllers.Remove(timeTickController);
            }
            else
            {
                Debug.LogWarning("Tried to remove which is not existed in the list.");
            }
        }

        /// <summary>
        /// Increases the timers by given parameter.
        /// </summary>
        /// <param name="timeToIncrease">Increase by value</param>
        public void UpdateTimers(float timeToIncrease)
        {
            foreach (var tickController in _timeTickControllers)
            {
                tickController.UpdateTimer(timeToIncrease);
            }
        }

        /// <summary>
        /// Finds pre-defined controller.
        /// Don't try to find custom one. Cache it before adding to list.
        /// </summary>
        /// <param name="timeIdentifier">Identifier of the controller</param>
        /// <param name="tickController">Requested time tick controller</param>
        /// <returns>True unless requested identifier is 'Custom'</returns>
        public bool GetPreDefinedTickController(TimeTickIdentifier timeIdentifier, out TimeTickController tickController)
        {
            if (timeIdentifier == TimeTickIdentifier.Custom)
            {
                Debug.LogWarning("Don't try to get custom one. Cache it before adding to list.");
                tickController = null;
                return false;
            }

            tickController = _timeTickControllers.FirstOrDefault(x => x.TimeIdentifier == timeIdentifier);
            return tickController != null;
        }

        /// <summary>
        /// Removes all the event listener from all of the controllers.
        /// </summary>
        public void RemoveAllListeners()
        {
            foreach (var tickData in _timeTickControllers)
            {
                tickData.RemoveAllListeners();
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace TimeTick
{
    public class TimeTickManager
    {
        private List<TimeTickController> _timeTickControllers;
        public List<TimeTickController> TimeTickControllers => _timeTickControllers;

        public TimeTickManager()
        {
            _timeTickControllers = new List<TimeTickController>
            {
                new(0, 1, TimeTickIdentifier.Second1),
                new(0, 2, TimeTickIdentifier.Second2),
                new(0, 3, TimeTickIdentifier.Second3),
                new(0, 5, TimeTickIdentifier.Second5),
                new(0, 10, TimeTickIdentifier.Second10),
            };
        }

        public void AddNewTickController(TimeTickController timeTickController)
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

        public void RemoveTickController(TimeTickController timeTickController)
        {
            if (_timeTickControllers == null || _timeTickControllers.Count == 0) return;
            if (timeTickController.TimeIdentifier != TimeTickIdentifier.Custom)
            {
                Debug.LogWarning("You shouldn't remove the default tick controllers.");
                return;
            }

            if (_timeTickControllers.Contains(timeTickController))
            {
                _timeTickControllers.Remove(timeTickController);
            }
            else
            {
                Debug.LogWarning("Tried to remove which is not existed in the list.");
            }
        }

        public void UpdateTimers(float timeToIncrease)
        {
            foreach (var tickController in _timeTickControllers)
            {
                tickController.UpdateTimer(timeToIncrease);
            }
        }

        public bool GetTickController(TimeTickIdentifier timeIdentifier, out TimeTickController tickController)
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

        public void RemoveAllListeners()
        {
            foreach (var tickData in _timeTickControllers)
            {
                tickData.RemoveAllListeners();
            }
        }
    }
}
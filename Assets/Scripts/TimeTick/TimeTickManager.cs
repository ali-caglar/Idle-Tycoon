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
                new(TimeTickIdentifier.Second1, 0, 1),
                new(TimeTickIdentifier.Second2, 0, 2),
                new(TimeTickIdentifier.Second3, 0, 3),
                new(TimeTickIdentifier.Second5, 0, 5),
                new(TimeTickIdentifier.Second10, 0, 10),
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
            _timeTickControllers.Remove(timeTickController);
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
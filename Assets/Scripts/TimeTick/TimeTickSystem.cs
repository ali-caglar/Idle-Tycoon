using System;
using Enums;
using UnityEngine;

namespace TimeTick
{
    public class TimeTickSystem : MonoBehaviour
    {
        private bool _isGameStarted;
        private TimeTickManager _timeTickManager;

        private void Awake()
        {
            _timeTickManager = new TimeTickManager();
        }

        private void Update()
        {
            if (!_isGameStarted) return;
            _timeTickManager.UpdateTimers(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _timeTickManager.RemoveAllListeners();
        }

        public void SubscribeToPreDefinedTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeIdentifier == TimeTickIdentifier.Custom) return;
            if (_timeTickManager.GetTickController(timeIdentifier, out TimeTickController tickController))
            {
                tickController.OnTimeTick += timeTickHandler;
            }
        }

        public void UnSubscribeFromPreDefinedTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeIdentifier == TimeTickIdentifier.Custom) return;
            if (_timeTickManager.GetTickController(timeIdentifier, out TimeTickController tickController))
            {
                tickController.OnTimeTick -= timeTickHandler;
            }
        }
    }
}
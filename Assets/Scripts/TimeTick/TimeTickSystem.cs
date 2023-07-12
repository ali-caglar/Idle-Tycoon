using System;
using Enums;
using UnityEngine;

namespace TimeTick
{
    public class TimeTickSystem : MonoBehaviour
    {
        #region PRIVATE FIELDS

        private bool _isGameStarted;
        private TimeTickManager _timeTickManager;

        #endregion

        #region PROPERTIES

        // Encapsulated for if any other class initialized before this class, field will be initialized by this property.
        private TimeTickManager TickManager
        {
            get
            {
                _timeTickManager ??= new TimeTickManager();
                return _timeTickManager;
            }
        }

        #endregion

        #region MONOBEHAVIOUR

        private void Awake()
        {
            _timeTickManager ??= new TimeTickManager();
        }

        private void Update()
        {
            if (!_isGameStarted) return;
            TickManager.UpdateTimers(Time.deltaTime);
        }

        private void OnDestroy()
        {
            TickManager.RemoveAllListeners();
        }

        #endregion

        #region PUBLIC METHODS

        public void AddNewTimeTick(TimeTickController timeTickController)
        {
            TickManager.AddNewCustomTickController(timeTickController);
        }

        public void RemoveTimeTick(TimeTickController timeTickController)
        {
            TickManager.RemoveCustomTickController(timeTickController);
        }

        public void SubscribeToPreDefinedTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeIdentifier == TimeTickIdentifier.Custom) return;
            if (TickManager.GetPreDefinedTickController(timeIdentifier, out TimeTickController tickController))
            {
                tickController.OnTimeTick += timeTickHandler;
            }
        }

        public void UnSubscribeFromPreDefinedTimeTick(TimeTickIdentifier timeIdentifier, Action timeTickHandler)
        {
            if (timeIdentifier == TimeTickIdentifier.Custom) return;
            if (TickManager.GetPreDefinedTickController(timeIdentifier, out TimeTickController tickController))
            {
                tickController.OnTimeTick -= timeTickHandler;
            }
        }

        #endregion
    }
}
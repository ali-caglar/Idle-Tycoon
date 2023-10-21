using System;
using UnityEditor;
using UnityEngine;

namespace Save.ScriptableObjects
{
    public abstract class BaseScriptableObjectWithSaveAndLoad : ScriptableObject
    {
        #region FIELDS

        [SerializeField, TextArea] protected string explanation;

        [Header("Default Datas")]
        [SerializeField] protected UUID id;

        #endregion

        #region UNITY METHODS

#if !UNITY_EDITOR
        protected void OnEnable() => OnBegin();
        protected void OnDisable() => OnEnd();
#endif

        #endregion

        #region PRIVATE METHODS

        protected virtual T LoadData<T>(T defaultData) where T : BaseDataModel<T>
        {
            var dataModelFromStorage = DataManager<T>.Load(defaultData.ID.uniqueID);
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultData.Clone(id);
                SaveData(dataModelFromStorage);
            }

            return dataModelFromStorage;
        }

        protected virtual void SaveData<T>(T dataModel) where T : BaseDataModel<T>
        {
            DataManager<T>.Save(dataModel, dataModel.ID.uniqueID);
        }

        protected virtual void HandleUniqueIdOnValidate()
        {
            if (string.IsNullOrEmpty(id.uniqueID))
            {
                ChangeID();
            }
        }

        #endregion

        #region ABSTRACT METHODS

        /// <summary>
        /// This method will work on enable
        /// </summary>
        protected abstract void OnBegin();

        /// <summary>
        /// This method will work on disable
        /// </summary>
        protected abstract void OnEnd();

        #endregion

        #region EDITOR CHECK

#if UNITY_EDITOR

        private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayStateChange;
        private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayStateChange;

        private void OnValidate()
        {
            HandleUniqueIdOnValidate();
        }

        [ContextMenu("Change ID")]
        private void ChangeID()
        {
            id.ChangeUniqueID();
        }

        private void OnPlayStateChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    OnBegin();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnEnd();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

#endif

        #endregion
    }
}
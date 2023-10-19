using System;
using UnityEditor;
using UnityEngine;

namespace Save
{
    public abstract class ScriptableObjectWithSaveAndLoad<TDataModel, TUserData> : ScriptableObject
        where TDataModel : BaseDataModel<TDataModel>
        where TUserData : BaseDataModel<TUserData>
    {
        #region FIELDS

        [SerializeField, TextArea] private string explanation;

        [Header("Default Datas")]
        [SerializeField] private UUID id;
        [SerializeField] private TDataModel defaultDataModel;
        [SerializeField] private TUserData defaultUserData;

        private TDataModel _dataModel;
        public TDataModel DataModel
        {
            get
            {
                if (_dataModel == null)
                {
                    LoadDataModel();
                }

                return _dataModel;
            }
        }

        private TUserData _userData;
        public TUserData UserData
        {
            get
            {
                if (_dataModel == null)
                {
                    LoadUserData();
                }

                return _userData;
            }
            set
            {
                _userData = value;
                SaveUserData(_userData);
            }
        }

        #endregion

        #region UNITY METHODS

#if !UNITY_EDITOR
        protected void OnEnable() => OnBegin();
        protected void OnDisable() => OnEnd();
#endif

        #endregion

        #region PRIVATE METHODS

        protected virtual void OnBegin()
        {
            Load();
        }

        protected virtual void OnEnd()
        {
            _dataModel = null;
            _userData = null;
        }

        #endregion

        #region SAVE&LOAD

        private void Load()
        {
            LoadDataModel();
            LoadUserData();
        }

        private void LoadDataModel()
        {
            var dataModelFromStorage = DataManager<TDataModel>.Load(defaultDataModel.ID.uniqueID);
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultDataModel.Clone(id);
                SaveDataModel(dataModelFromStorage);
            }

            _dataModel = dataModelFromStorage;
        }

        private void LoadUserData()
        {
            var dataModelFromStorage = DataManager<TUserData>.Load(defaultUserData.ID.uniqueID);
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultUserData.Clone(id);
                SaveUserData(dataModelFromStorage);
            }

            _userData = dataModelFromStorage;
        }

        private void SaveDataModel(TDataModel dataModel)
        {
            DataManager<TDataModel>.Save(dataModel, dataModel.ID.uniqueID);
        }

        private void SaveUserData(TUserData userData)
        {
            DataManager<TUserData>.Save(userData, userData.ID.uniqueID);
        }

        #endregion

        #region EDITOR CHECK

#if UNITY_EDITOR

        protected void OnEnable() => EditorApplication.playModeStateChanged += OnPlayStateChange;
        protected void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayStateChange;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id.uniqueID))
            {
                ChangeID();
            }

            var idCopy = id.Clone();
            defaultDataModel.ID = idCopy;
            defaultUserData.ID = idCopy;
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
using Enums.ID;
using UnityEngine;
using Utility;

namespace Save
{
    public abstract class ScriptableObjectWithSaveAndLoad<TDataModel, TUserData> : ScriptableObject,
        IResetOnPlaymodeExit
        where TDataModel : BaseDataModel<TDataModel>
        where TUserData : BaseDataModel<TUserData>
    {
        #region FIELDS

        [SerializeField, TextArea] private string explanation;

        [Header("ID")]
        [SerializeField] private WorldName worldName;
        [SerializeField] private RegionName regionName;
        [SerializeField] private UUID id;

        [Header("Default Datas")]
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

        #region INTERFACE METHODS

        public void PlaymodeExitReset()
        {
            _dataModel = null;
            _userData = null;
        }

        #endregion

        #region SAVE&LOAD

        public void Load()
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

        #region VALIDATE

#if UNITY_EDITOR

        private void HandleID()
        {
            id.worldNumber = (uint)worldName;
            id.regionNumber = (uint)regionName;
            if (string.IsNullOrEmpty(id.uniqueID))
            {
                id.ChangeUniqueID();
            }
        }

        private void OnValidate()
        {
            HandleID();
            var idCopy = id.Clone();
            defaultDataModel.ID = idCopy;
            defaultUserData.ID = idCopy;
        }

#endif

        #endregion
    }
}
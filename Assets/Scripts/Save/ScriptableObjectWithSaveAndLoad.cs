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
            var dataModelFromStorage = DataManager<TDataModel>.Load();
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultDataModel.Clone();
                SaveDataModel(dataModelFromStorage);
            }

            _dataModel = dataModelFromStorage;
        }

        private void LoadUserData()
        {
            var dataModelFromStorage = DataManager<TUserData>.Load();
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultUserData.Clone();
                SaveUserData(dataModelFromStorage);
            }

            _userData = dataModelFromStorage;
        }

        private void SaveDataModel(TDataModel dataModel)
        {
            DataManager<TDataModel>.Save(dataModel);
        }

        private void SaveUserData(TUserData userData)
        {
            DataManager<TUserData>.Save(userData);
        }

        #endregion
    }
}
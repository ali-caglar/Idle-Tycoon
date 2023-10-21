using UnityEngine;

namespace Save.ScriptableObjects
{
    public abstract class ScriptableObjectWithSaveAndLoad<TDataModel> : BaseScriptableObjectWithSaveAndLoad
        where TDataModel : BaseDataModel<TDataModel>
    {
        #region FIELDS

        [SerializeField] protected TDataModel defaultDataModel;

        protected TDataModel _dataModel;
        public TDataModel DataModel
        {
            get
            {
                if (_dataModel == null || string.IsNullOrEmpty(_dataModel.ID.uniqueID))
                {
                    LoadDataModel();
                }

                return _dataModel;
            }
        }

        #endregion

        #region PRIVATE METHODS

        protected override void OnBegin()
        {
            Load();
        }

        protected override void OnEnd()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            _dataModel = null;
        }

        protected override void HandleUniqueIdOnValidate()
        {
            base.HandleUniqueIdOnValidate();

            var idCopy = id.Clone();
            defaultDataModel.ID = idCopy;
        }

        #endregion

        #region SAVE&LOAD

        protected virtual void Load()
        {
            LoadDataModel();
        }

        private void LoadDataModel()
        {
            _dataModel = LoadData(defaultDataModel);
        }

        #endregion
    }

    public abstract class ScriptableObjectWithSaveAndLoad<TDataModel, TUserData> : ScriptableObjectWithSaveAndLoad<TDataModel>
        where TDataModel : BaseDataModel<TDataModel>
        where TUserData : BaseDataModel<TUserData>
    {
        #region FIELDS

        [SerializeField] protected TUserData defaultUserData;

        protected TUserData _userData;
        public TUserData UserData
        {
            get
            {
                if (_userData == null || string.IsNullOrEmpty(_userData.ID.uniqueID))
                {
                    LoadUserData();
                }

                return _userData;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void SaveUserData()
        {
            SaveData(_userData);
        }

        #endregion

        #region PRIVATE METHODS

        protected override void HandleUniqueIdOnValidate()
        {
            base.HandleUniqueIdOnValidate();

            var idCopy = id.Clone();
            defaultUserData.ID = idCopy;
        }

        protected override void Reset()
        {
            base.Reset();
            _userData = null;
        }

        #endregion

        #region SAVE&LOAD

        protected override void Load()
        {
            base.Load();
            LoadUserData();
        }

        private void LoadUserData()
        {
            _userData = LoadData(defaultUserData);
        }

        #endregion
    }
}
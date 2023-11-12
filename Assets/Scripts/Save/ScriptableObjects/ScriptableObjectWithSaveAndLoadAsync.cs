using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Save.ScriptableObjects
{
    public abstract class ScriptableObjectWithSaveAndLoadAsync<TDataModel> : BaseScriptableObjectWithSaveAndLoadAsync
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
                    _dataModel = LoadData(defaultDataModel);
                }

                return _dataModel;
            }
        }

        #endregion

        #region PRIVATE METHODS

        protected override async void OnBegin()
        {
            await InitializeAsync();
        }

        protected override void OnEnd()
        {
            Reset();
        }

        protected virtual void Reset()
        {
            _dataModel = null;
            if (CancelTokenSource != null)
            {
                CancelTokenSource.Cancel();
                CancelTokenSource.Dispose();
                CancelTokenSource = null;
            }
        }

        protected override void HandleUniqueIdOnValidate()
        {
            base.HandleUniqueIdOnValidate();

            var idCopy = id.Clone();
            defaultDataModel.ID = idCopy;
        }

        #endregion

        #region SAVE&LOAD

        public virtual async UniTask InitializeAsync()
        {
            CancelTokenSource ??= new CancellationTokenSource();
            await LoadDataModel();
        }

        private async UniTask LoadDataModel()
        {
            _dataModel = await LoadDataAsync(defaultDataModel);
        }

        #endregion
    }

    public abstract class ScriptableObjectWithSaveAndLoadAsync<TDataModel, TUserData>
        : ScriptableObjectWithSaveAndLoadAsync<TDataModel>
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
                    _userData = LoadData(defaultUserData);
                }

                return _userData;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public async UniTask SaveUserData()
        {
            await SaveDataAsync(_userData);
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

        public override async UniTask InitializeAsync()
        {
            var dataModelUniTask = base.InitializeAsync();
            var userDataUniTask = LoadUserData();
            await UniTask.WhenAll(dataModelUniTask, userDataUniTask);
        }

        private async UniTask LoadUserData()
        {
            _userData = await LoadDataAsync(defaultUserData);
        }

        #endregion
    }
}
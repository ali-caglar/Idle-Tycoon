using System.Threading;
using Cysharp.Threading.Tasks;

namespace Save.ScriptableObjects
{
    public abstract class BaseScriptableObjectWithSaveAndLoadAsync : BaseSOWithSaveAndLoad
    {
        #region FIELDS

        protected CancellationTokenSource CancelTokenSource { get; set; }

        #endregion

        #region PRIVATE METHODS

        protected async UniTask<T> LoadDataAsync<T>(T defaultData) where T : BaseDataModel<T>
        {
            var dataModelFromStorage = await DataManager<T>.LoadAsync(defaultData.ID.uniqueID, CancelTokenSource);
            if (dataModelFromStorage == null)
            {
                dataModelFromStorage = defaultData.Clone(id);
                await SaveDataAsync(dataModelFromStorage);
            }

            return dataModelFromStorage;
        }

        protected async UniTask SaveDataAsync<T>(T dataModel) where T : BaseDataModel<T>
        {
            await DataManager<T>.SaveAsync(dataModel, dataModel.ID.uniqueID, CancelTokenSource);
        }

        #endregion

        #region ABSTRACT METHODS

        /// <summary>
        /// Initialize Method by async
        /// </summary>
        public abstract UniTask InitializeAsync();

        #endregion
    }
}
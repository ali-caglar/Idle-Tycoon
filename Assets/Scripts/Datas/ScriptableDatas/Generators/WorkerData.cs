using Datas.DataModels.Workers;
using Datas.ScriptableDatas.Common;
using UnityEngine;

namespace Datas.ScriptableDatas.Generators
{
    [CreateAssetMenu(fileName = "Worker Data", menuName = "Datas/Generator/Worker Data", order = 1)]
    public class WorkerData : DataHandlerScriptableObject
    {
        #region SERIALIZED PRIVATE FIELDS

        [SerializeField] private WorkerDataModel dataModelOnDeploy;

        #endregion

        #region PRIVATE FIELDS

        private WorkerDataModel _dataModelToUse;
        private UserDataForWorkerDataModel _userData;

        #endregion

        #region PROPERTIES

        public WorkerDataModel DataModel
        {
            get
            {
                if (_dataModelToUse == null)
                {
                    Load();
                }

                return _dataModelToUse;
            }
        }

        public UserDataForWorkerDataModel UserData
        {
            get
            {
                if (_userData == null)
                {
                    Load();
                }

                return _userData;
            }
        }

        #endregion

        #region LIFECYCLE

        private void OnEnable()
        {
            Load();
        }

        private void OnDisable()
        {
            _userData = null;
            _dataModelToUse = null;
        }

        #endregion

        #region Save&Load

        public override void Load()
        {
            // Check remote for if any data is changed
            // else load data from local/storage
            // load user data
        }

        public override void Save()
        {
            // Save remote data to storage
            // save user data
        }

        #endregion

        #region Editor

#if UNITY_EDITOR

        private void OnValidate()
        {
            var identifierID = dataModelOnDeploy.identifierID;
            var productionType = dataModelOnDeploy.bonusDataModel.bonusType.ToString().ToLower();
            var displayName = dataModelOnDeploy.displayName.ToLower();
            if (string.IsNullOrEmpty(identifierID) || 
                (!identifierID.Contains(productionType) && !identifierID.Contains(displayName)))
            {
                dataModelOnDeploy.identifierID = $"worker-{productionType}-{displayName}-{System.Guid.NewGuid()}";
            }
        }

#endif

        #endregion
    }
}
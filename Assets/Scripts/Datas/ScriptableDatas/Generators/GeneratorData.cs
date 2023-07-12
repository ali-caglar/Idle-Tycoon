using Datas.DataModels.Generators;
using Datas.ScriptableDatas.Common;
using UnityEngine;

namespace Datas.ScriptableDatas.Generators
{
    [CreateAssetMenu(fileName = "Generator Data", menuName = "Datas/Generator/Generator Data", order = 0)]
    public class GeneratorData : DataHandlerScriptableObject
    {
        #region SERIALIZED PRIVATE FIELDS

        [SerializeField] private GeneratorDataModel dataModelOnDeploy;

        #endregion

        #region PRIVATE FIELDS

        private GeneratorDataModel _dataModelToUse;
        private UserDataForGeneratorDataModel _userData;

        #endregion

        #region PROPERTIES

        public GeneratorDataModel DataModel
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

        public UserDataForGeneratorDataModel UserData
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
            var productionType = dataModelOnDeploy.profitDataModel.profitCurrencyType.ToString().ToLower();
            if (string.IsNullOrEmpty(identifierID) || !identifierID.Contains(productionType))
            {
                dataModelOnDeploy.identifierID = $"generator-{productionType}-{System.Guid.NewGuid()}";
            }
        }

#endif

        #endregion
    }
}
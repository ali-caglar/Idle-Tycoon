using Datas.DataModels.Generators;
using Datas.ScriptableDatas.Common;
using UnityEngine;

namespace Datas.ScriptableDatas.Generators
{
    [CreateAssetMenu(fileName = "Generator Data", menuName = "Datas/Generator/Generator Data", order = 0)]
    public class GeneratorData : DataHandlerScriptableObject
    {
        [SerializeField] private GeneratorDataModel dataModelOnDeploy;

        private GeneratorDataModel _dataModelToUse;
        private UserDataForGeneratorDataModel _userData;

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

#endif

        #endregion
    }
}
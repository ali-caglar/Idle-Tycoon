using System.Linq;
using Cysharp.Threading.Tasks;
using Save.ScriptableObjects;
using UnityEngine;

namespace Save
{
    public class SaveLoader : MonoBehaviour
    {
        [SerializeField] private BaseScriptableObjectWithSaveAndLoad[] saveDatas;
        [SerializeField] private BaseScriptableObjectWithSaveAndLoadAsync[] asyncSaveDatas;

        public async UniTask LoadAllSaveDatas()
        {
            foreach (var saveData in saveDatas)
            {
                saveData.Initialize();
            }

            var asyncSaveTask = Enumerable.Select(asyncSaveDatas, asyncSaveData => asyncSaveData.InitializeAsync());
            await UniTask.WhenAll(asyncSaveTask);
        }

#if UNITY_EDITOR

        [ContextMenu("Find All Save Datas And Cache")]
        private void FindAllSaveDatasAndCache()
        {
            saveDatas = Resources.FindObjectsOfTypeAll<BaseScriptableObjectWithSaveAndLoad>();
            asyncSaveDatas = Resources.FindObjectsOfTypeAll<BaseScriptableObjectWithSaveAndLoadAsync>();
        }

#endif
    }
}
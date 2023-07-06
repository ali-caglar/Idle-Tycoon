using UnityEngine;

namespace Datas.ScriptableDatas.Common
{
    public abstract class DataHandlerScriptableObject : ScriptableObject
    {
        public abstract void Load();
        public abstract void Save();
    }
}
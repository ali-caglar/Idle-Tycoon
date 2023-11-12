using System;
using Save.ScriptableObjects;

namespace Save.Demo.Scripts
{
    public class DemoSaveAndLoadAsyncSaveDataWithSaveAndLoadUserData : ScriptableObjectWithSaveAndLoadAsync<DemoAsyncModel1, DemoAsyncModel2>
    {
    }

    [Serializable]
    public class DemoAsyncModel1 : BaseDataModel<DemoAsyncModel1>
    {
        public string alias;
        public int age;
    }

    [Serializable]
    public class DemoAsyncModel2 : BaseDataModel<DemoAsyncModel2>
    {
        public ulong attemptCount;
    }
}
using System;

namespace Save
{
    [Serializable]
    public abstract class BaseDataModel<T>
    {
        public string identifier;
        public uint worldNumber;
        public uint regionNumber;

        public abstract T Clone();
    }
}
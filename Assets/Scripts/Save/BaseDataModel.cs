using System;
using Utility.Attributes;

namespace Save
{
    [Serializable]
    public abstract class BaseDataModel<T>
    {
        [ReadOnly] public UUID ID;

        public abstract T Clone(UUID id);
    }
}
using System;
using Save.DataServices;
using Utility.Attributes;

namespace Save
{
    [Serializable]
    public abstract class BaseDataModel<T>
    {
        [ReadOnly] public UUID ID;

        private ISerializationService SerializationService => DataManager<T>.SerializationService;

        /// <summary>
        /// Clones this object (not copies private fields)
        /// </summary>
        /// <param name="id">Id to clone</param>
        /// <returns>Deep copy</returns>
        public T Clone(UUID id)
        {
            ID = id.Clone();

            string json = SerializationService.ConvertToJson(this);
            T returnedData = SerializationService.ConvertFromJson<T>(json);
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }
    }
}
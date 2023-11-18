using System;
using Save.DataServices;
using Utility.Attributes;
using Zenject;

namespace Save
{
    [Serializable]
    public abstract class BaseDataModel<T>
    {
        [ReadOnly] public UUID ID;

        /// <summary>
        /// Json serialize service
        /// </summary>
        [Inject]
        private ISerializationService _serializationService;

        /// <summary>
        /// Clones this object (not copies private fields)
        /// </summary>
        /// <param name="id">Id to clone</param>
        /// <returns>Deep copy</returns>
        public T Clone(UUID id)
        {
            ID = id.Clone();

            string json = _serializationService.ConvertToJson(this);
            T returnedData = _serializationService.ConvertFromJson<T>(json);
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }
    }
}
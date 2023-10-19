using System;
using Save.DataServices;
using Utility.Attributes;

namespace Save
{
    [Serializable]
    public abstract class BaseDataModel<T>
    {
        [ReadOnly] public UUID ID;

        private IDataService _dataService = new NewtonsoftDataService();

        /// <summary>
        /// Clones this object (not copies private fields)
        /// </summary>
        /// <param name="id">Id to clone</param>
        /// <returns>Deep copy</returns>
        public T Clone(UUID id)
        {
            ID = id.Clone();

            string json = _dataService.ConvertToJson(this);
            T returnedData = _dataService.ConvertFromJson<T>(json);
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }
    }
}
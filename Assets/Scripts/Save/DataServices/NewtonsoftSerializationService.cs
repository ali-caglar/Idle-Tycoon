using Newtonsoft.Json;

namespace Save.DataServices
{
    public class NewtonsoftSerializationService : ISerializationService
    {
        public string ConvertToJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public T ConvertFromJson<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }
    }
}
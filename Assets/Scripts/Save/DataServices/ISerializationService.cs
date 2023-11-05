namespace Save.DataServices
{
    public interface ISerializationService
    {
        string ConvertToJson<T>(T data);
        T ConvertFromJson<T>(string json);
    }
}
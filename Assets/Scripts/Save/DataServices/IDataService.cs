namespace Save.DataServices
{
    public interface IDataService
    {
        string ConvertToJson<T>(T data);
        T ConvertFromJson<T>(string json);
    }
}
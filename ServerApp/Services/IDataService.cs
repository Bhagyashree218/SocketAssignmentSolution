namespace ServerApp.Services;
public interface IDataService
{
    int? GetValue(string setName, string keyName);
}

namespace ServerApp.Services;
public interface IEncryptionService
{
    string Encrypt(string text);
    string Decrypt(string text);
}

namespace StoneAssemblies.Keycloak.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Decrypt(string encryptedText);

        string Encrypt(string plainText);
    }
}
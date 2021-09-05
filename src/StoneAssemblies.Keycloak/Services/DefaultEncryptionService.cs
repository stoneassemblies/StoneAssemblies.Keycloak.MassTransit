namespace StoneAssemblies.Keycloak.Services
{
    using StoneAssemblies.Keycloak.Services.Interfaces;

    public class DefaultEncryptionService : IEncryptionService
    {
        public string Decrypt(string encryptedText)
        {
            return encryptedText;
        }

        public string Encrypt(string plainText)
        {
            return plainText;
        }
    }
}
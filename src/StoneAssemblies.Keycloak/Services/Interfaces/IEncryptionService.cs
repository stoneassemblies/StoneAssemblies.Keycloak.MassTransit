namespace StoneAssemblies.Keycloak.Services.Interfaces
{
    /// <summary>
    /// The EncryptionService interface.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// The decrypt.
        /// </summary>
        /// <param name="encryptedText">
        /// The encrypted text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Decrypt(string encryptedText);

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="plainText">
        /// The plain text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Encrypt(string plainText);
    }
}
namespace StoneAssemblies.Keycloak.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The aes encryption service.
    /// </summary>
    /// <remarks>
    ///     Implemented from
    ///     https://zenu.wordpress.com/2011/09/21/aes-128bit-cross-platform-java-and-c-encryption-compatibility/
    ///     by Joseph Ssenyange
    /// </remarks>
    public class AesEncryptionService : IEncryptionService
    {
        private readonly string secret;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AesEncryptionService" /> class.
        /// </summary>
        /// <param name="secret">
        ///     The secret.
        /// </param>
        public AesEncryptionService(string secret)
        {
            this.secret = secret;
        }

        /// <summary>
        ///     The decrypt.
        /// </summary>
        /// <param name="encryptedText">
        ///     The encrypted text.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string Decrypt(string encryptedText)
        {
            return this.Decrypt(encryptedText, this.secret);
        }

        /// <summary>
        ///     Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <param name="key">Secret key</param>
        /// <returns>Base64 encoded string</returns>
        public string Encrypt(string plainText, string key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(this.Encrypt(plainBytes, this.GetRijndaelManaged(key)));
        }

        public string Encrypt(string plainText)
        {
            return this.Encrypt(plainText, this.secret);
        }

        private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        ///     Decrypts a base64 encoded string using the given key (AES 128bit key and a Chain Block Cipher)
        /// </summary>
        /// <param name="encryptedText">Base64 Encoded String</param>
        /// <param name="key">Secret Key</param>
        /// <returns>Decrypted String</returns>
        private string Decrypt(string encryptedText, string key)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(this.Decrypt(encryptedBytes, this.GetRijndaelManaged(key)));
        }

        private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
                       {
                           Mode = CipherMode.CBC,
                           Padding = PaddingMode.PKCS7,
                           KeySize = 128,
                           BlockSize = 128,
                           Key = keyBytes,
                           IV = keyBytes
                       };
        }
    }
}
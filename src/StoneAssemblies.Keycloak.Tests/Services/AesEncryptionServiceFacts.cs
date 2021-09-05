namespace StoneAssemblies.Keycloak.Tests.Services
{
    using StoneAssemblies.Keycloak.Services;

    using Xunit;

    public class AesEncryptionServiceFacts
    {
        public class The_Decrypt_Method
        {
            [Fact]
            public void Decrypt_An_Encrypted_Text_Correctly()
            {
                var aesEncryptionService = new AesEncryptionService("sOme*ShaREd*SecreT");

                var plainText = "My Clear Text";
                var encryptedText = aesEncryptionService.Encrypt(plainText);
                var decryptedText = aesEncryptionService.Decrypt(encryptedText);

                Assert.Equal(plainText, decryptedText);
            }
        }

        public class The_Encrypt_Method
        {
            [Fact]
            public void Encrypt_A_Plain_Text_Correctly()
            {
                var aesEncryptionService = new AesEncryptionService("sOme*ShaREd*SecreT");

                var plainText = "My Clear Text";
                var encryptedText = aesEncryptionService.Encrypt(plainText);
                var decryptedText = aesEncryptionService.Decrypt(encryptedText);

                Assert.Equal(plainText, decryptedText);
            }
        }
    }
}
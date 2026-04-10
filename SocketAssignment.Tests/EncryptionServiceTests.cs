using ServerApp.Services;

namespace SocketAssignment.Tests
{
    public class EncryptionServiceTests
    {
        [Fact]
        public void EncryptDecrypt_Works()
        {
            var service = new EncryptionService("1234567890123456", "1234567890123456");

            var encrypted = service.Encrypt("hello");
            var decrypted = service.Decrypt(encrypted);

            Assert.Equal("hello", decrypted);
        }
    }
}

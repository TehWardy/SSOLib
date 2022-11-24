using Security.Data.Interfaces;

namespace Security.Data.Brokers.Encryption
{
    public class PasswordEncryptionBrokerAESHMAC : IPasswordEncryptionBroker
    {
        private readonly ISymmetricCrypto<string> crypto;

        public PasswordEncryptionBrokerAESHMAC(ISymmetricCrypto<string> crypto)
        {
            this.crypto = crypto;
        }

        public string Encrypt(string password)
            => crypto.Encrypt(password);

        public bool EncryptedAndPlainTextAreEqual(string encrypted, string plainText)
            => crypto.Decrypt(encrypted) == plainText;
    }
}

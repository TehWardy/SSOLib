namespace Security.Data.Brokers.Encryption
{
    public interface IPasswordEncryptionBroker
    {
        string Encrypt(string password);
        bool EncryptedAndPlainTextAreEqual(string encrypted, string plainText);
    }
}
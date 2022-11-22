namespace Security.Data.Brokers.Encryption
{
    public interface IPasswordEncryptionBroker
    {
        string Encrypt(string password);
    }
}
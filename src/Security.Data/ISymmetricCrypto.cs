namespace Security.Data.Interfaces
{
    public interface ISymmetricCrypto<T>
    {
        string Encrypt(T source, string key);
        string Encrypt(T source);

        T Decrypt(string source, string key);
        T Decrypt(string source);
    }
}
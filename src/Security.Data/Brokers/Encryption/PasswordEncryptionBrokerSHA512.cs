using System;
using System.Security.Cryptography;
using System.Text;

namespace Security.Data.Brokers.Encryption
{
	public class PasswordEncryptionBrokerSHA512 : IPasswordEncryptionBroker
	{
		public PasswordEncryptionBrokerSHA512()
		{
		}

        public string Encrypt(string password)
        {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = SHA512.HashData(inputBytes);
            return Encoding.UTF8.GetString(hashBytes);
        }

        public bool EncryptedAndPlainTextAreEqual(string encrypted, string plainText)
        {
            var inputBytes = Encoding.UTF8.GetBytes(plainText);
            var hashBytes = SHA512.HashData(inputBytes);
            string hashedString = Encoding.UTF8.GetString(hashBytes);
            return hashedString == encrypted;
        }
    }
}


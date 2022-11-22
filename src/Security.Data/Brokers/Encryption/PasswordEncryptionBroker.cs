using Security.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Data.Brokers.Encryption
{
    public class PasswordEncryptionBroker : IPasswordEncryptionBroker
    {
        private readonly ICrypto<string> crypto;

        public PasswordEncryptionBroker(ICrypto<string> crypto)
        {
            this.crypto = crypto;
        }

        public string Encrypt(string password)
            => crypto.Encrypt(password);
    }
}

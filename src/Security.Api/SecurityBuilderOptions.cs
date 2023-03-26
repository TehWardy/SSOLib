using Microsoft.Extensions.DependencyInjection;
using Security.Data;
using Security.Data.Brokers.Encryption;
using Security.Data.EF;
using Security.Data.EF.MSSQL;
using Security.Data.Interfaces;

namespace Security.Api
{
    public class SecurityBuilderOptions
    {
        private readonly IServiceCollection serviceDescriptors;

        public SecurityBuilderOptions(IServiceCollection serviceDescriptors)
        {
            this.serviceDescriptors = serviceDescriptors;
        }

        public SecurityBuilderOptions UseSHA512PasswordEncryption()
        {
            serviceDescriptors.AddTransient<IPasswordEncryptionBroker, PasswordEncryptionBrokerSHA512>();
            return this;
        }

        public SecurityBuilderOptions UseAESHMMACPasswordEncryption(string decryptionKey)
        {
            serviceDescriptors.AddTransient<ISymmetricCrypto<string>>(_ => new AesCrypto<string>(decryptionKey));
            serviceDescriptors.AddTransient<IPasswordEncryptionBroker, PasswordEncryptionBrokerAESHMAC>();
            return this;
        }

        public SecurityBuilderOptions UseMSSQLProvider(string connectionString)
        {
            serviceDescriptors.AddSingleton<ISecurityModelBuildProvider>(new SecurityMSSQLModelBuildProvider(connectionString));
            serviceDescriptors.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName = "dbo";
                options.TableName = "Sessions";
            });

            return this;
        }

        public SecurityBuilderOptions UseSQLiteProvider(string connectionString)
        {
            serviceDescriptors.AddSingleton<ISecurityModelBuildProvider>(new SecuritySQLiteModelBuildProvider(connectionString));
            return this;
        }
    }
}


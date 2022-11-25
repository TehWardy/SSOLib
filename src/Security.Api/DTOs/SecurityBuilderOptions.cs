using System;
using Microsoft.Extensions.Configuration;
using Security.Data;
using Security.Data.Brokers.Encryption;
using Security.Data.EF;
using Security.Data.EF.MSSQL;
using Security.Data.EF.SQLite;
using Microsoft.Extensions.Caching.SqlServer;
using Security.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Security.Api.DTOs
{
	public class SecurityBuilderOptions
	{
        private readonly IServiceCollection serviceDescriptors;
        private readonly IConfiguration configuration;

        private IDictionary<string, string> settings { get; set; }

        public SecurityBuilderOptions(IServiceCollection serviceDescriptors,
            IConfiguration configuration)
		{
            this.serviceDescriptors = serviceDescriptors;
            this.configuration = configuration;
            settings = new Dictionary<string, string>()
            {
                { "passwordEncryption", "SHA512" },
                { "databaseProvider", configuration.GetSection("Settings").GetValue<string>("DbType") ?? "mssql" }
            };
        }

        public SecurityBuilderOptions UseSHA512PasswordEncryption()
        {
            settings["passwordEncryption"] = "SHA512";
            return this;
        }

        public SecurityBuilderOptions UseAESHMMACPasswordEncryption()
        {
            settings["passwordEncryption"] = "AESHMAC";
            return this;
        }

        public SecurityBuilderOptions UseMSSQLProvider()
        {
            settings["databaseProvider"] = "mssql";
            return this;
        }

        public SecurityBuilderOptions UseSQLiteProvider()
        {
            settings["databaseProvider"] = "sqlite";
            return this;
        }

        public void Build()
        {
            SetupEncryptionAlgorithm();
            SetupDatabaseProvider();
        }

        private void SetupEncryptionAlgorithm()
        {
            if (settings["passwordEncryption"] == "SHA512")
                serviceDescriptors.AddTransient<IPasswordEncryptionBroker, PasswordEncryptionBrokerSHA512>();
            else
            {
                serviceDescriptors.AddTransient<ISymmetricCrypto<string>>(_ => new AesCrypto<string>(configuration.GetSection("Settings")["DecryptionKey"]));
                serviceDescriptors.AddTransient<IPasswordEncryptionBroker, PasswordEncryptionBrokerAESHMAC>();
            }
        }

        private void SetupDatabaseProvider()
        {
            if (settings["databaseProvider"] == "mssql")
            {
                serviceDescriptors.AddTransient<ISecurityModelBuildProvider, SecurityMSSQLModelBuildProvider>();

                serviceDescriptors.AddDistributedSqlServerCache(options => {
                    options.ConnectionString = configuration.GetConnectionString("SSO");
                    options.SchemaName = "dbo";
                    options.TableName = "Sessions";
                });
            }
            else
            {
                serviceDescriptors.AddTransient<ISecurityModelBuildProvider, SecuritySQLiteModelBuildProvider>();
            }
        }
	}
}


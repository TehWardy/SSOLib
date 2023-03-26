using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.UserManager;
using System;
using System.IO;

namespace SecuritySQLite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ENV_")
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // configure DI for stack
            builder.Services.AddAspNetCore();
            builder.Services.AddMetadata();

            builder.Services.AddSecurity(optionsBuilder => 
            {
                optionsBuilder.UseSQLiteProvider(config.GetConnectionString("SSO"))
                    .UseSHA512PasswordEncryption();
            });

            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromMinutes(60);
            });

            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

            builder.Logging.ClearProviders();
            builder.Logging.AddSimpleConsole();

            var app = builder.Build();
            app.UseSession();
            app.UseTheFramework();
            app.Run();
        }
    }
}
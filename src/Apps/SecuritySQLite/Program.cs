using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.Data.EF.MSSQL;
using Security.Data.EF;
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

            // configure DI for stack
            builder.Services.AddAspNetCore();
            builder.Services.AddMetadata();

            builder.Services.AddSecurity(builder.Configuration, optionsBuilder
                => optionsBuilder.UseSQLiteProvider()
                    .UseSHA512PasswordEncryption());

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
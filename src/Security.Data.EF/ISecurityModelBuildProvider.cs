using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Security.Data.EF
{
    public interface ISecurityModelBuildProvider
    {
        void Configure(IConfiguration configuration, DbContextOptionsBuilder optionsBuilder);
        void Create(ModelBuilder modelBuilder);
        void MigrateDatabase(DatabaseFacade database);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Security.Data.EF
{
    public interface ISecurityModelBuildProvider
    {
        void Configure(DbContextOptionsBuilder optionsBuilder);
        void Create(ModelBuilder modelBuilder);
        void MigrateDatabase(DatabaseFacade database);
    }
}
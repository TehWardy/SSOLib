using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Security.Objects.Entities;

namespace Security.Data.EF.MSSQL;

public partial class SecurityMSSQLModelBuildProvider : ISecurityModelBuildProvider
{
    public void MigrateDatabase(DatabaseFacade database)
        => database.Migrate();

    public void Create(ModelBuilder modelBuilder)
    {
        ConfigureSecurityModel(modelBuilder);

        var cascadingRelationships = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var relationship in cascadingRelationships)
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    public void Configure(IConfiguration configuration, DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = configuration.GetConnectionString("SSO");
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Security.Data.EF.MSSQL"));
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace Security.Data.EF.MSSQL;

public partial class SecurityMSSQLModelBuildProvider : ISecurityModelBuildProvider
{
    readonly string connectionString;

    public SecurityMSSQLModelBuildProvider(string connectionString) =>
        this.connectionString = connectionString;

    public void MigrateDatabase(DatabaseFacade database) => 
        database.Migrate();

    public void Create(ModelBuilder modelBuilder)
    {
        ConfigureSecurityModel(modelBuilder);

        var cascadingRelationships = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var relationship in cascadingRelationships)
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    public void Configure(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Security.Data.EF.MSSQL"));
}


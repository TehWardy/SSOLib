using System;
namespace Security.Data.EF.Interfaces
{
    public interface IIdentitySSODbContextFactory
    {
        IdentitySSODbContext CreateDbContext();
    }
}


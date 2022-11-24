namespace Security.Data.EF.Interfaces
{
    public interface ISSODbContextFactory
    {
        SSODbContext CreateDbContext(bool ignoreFilters = false);
    }
}
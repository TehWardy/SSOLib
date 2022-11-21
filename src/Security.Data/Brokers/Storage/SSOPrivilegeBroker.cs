using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Storage
{
    public class SSOPrivilegeBroker : ISSOPrivilegeBroker
    {
        ISSODbContextFactory contextFactory;

        public SSOPrivilegeBroker(ISSODbContextFactory contextFactory)
            => this.contextFactory = contextFactory;

        public IQueryable<SSOPrivilege> GetPrivileges()
            => contextFactory.CreateDbContext().GetPrivileges();
    }
}
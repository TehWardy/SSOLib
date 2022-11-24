using Security.Data.EF;
using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Authentication
{
    public class IdentityBroker : IIdentityBroker
    {
        private readonly ISSODbContextFactory dbContextFactory;

        public IdentityBroker(ISSODbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public SSOUser Me()
            => ((IdentitySSODbContext)dbContextFactory.CreateDbContext(true)).GetCurrentUser();
    }
}

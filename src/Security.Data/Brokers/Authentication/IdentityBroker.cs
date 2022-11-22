using Security.Data.EF.Interfaces;
using Security.Objects.Entities;

namespace Security.Data.Brokers.Authentication
{
    public class IdentityBroker : IIdentityBroker
    {
        private readonly IIdentitySSODbContextFactory dbContextFactory;

        public IdentityBroker(IIdentitySSODbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public SSOUser Me()
            => dbContextFactory.CreateDbContext().GetCurrentUser();
    }
}

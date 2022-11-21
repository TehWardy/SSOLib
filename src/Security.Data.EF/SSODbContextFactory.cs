using Microsoft.Extensions.Configuration;
using Security.Data.EF.Interfaces;
using Security.Objects;

namespace Security.Data.EF
{
    // This Wrapper in case we want to take a different approach to the EF factory for our context instancing 
    // or just want a single place to call the ctor to avoid affecting a lot of files when the ctor changes.
    public class SSODbContextFactory : ISSODbContextFactory
    {
        /*  
        readonly IDbContextFactory<SSODbContext> externalFactory;

        public SSODbContextFactory(IDbContextFactory<SSODbContext> externalFactory)
            => this.externalFactory = externalFactory;

        public SSODbContext CreateDbContext()
            => externalFactory.CreateDbContext();
        */

        private readonly IConfiguration configuration;
        private readonly ISecurityModelBuildProvider modelBuildProvider;

        public SSODbContextFactory(IConfiguration configuration, ISecurityModelBuildProvider modelBuildProvider)
        {
            this.configuration = configuration;
            this.modelBuildProvider = modelBuildProvider;
        }

        public SSODbContext CreateDbContext()
            => new(configuration, null, modelBuildProvider);
    }
}
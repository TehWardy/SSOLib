using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.EF.Interfaces;
using Security.Objects;

namespace Security.Data.EF
{
    // This Wrapper in case we want to take a different approach to the EF factory for our context instancing 
    // or just want a single place to call the ctor to avoid affecting a lot of files when the ctor changes.
    public class SSODbContextFactory : ISSODbContextFactory
    {
        private readonly IConfiguration configuration;
        private readonly ISecurityModelBuildProvider modelBuildProvider;
        private readonly IServiceProvider provider;

        public SSODbContextFactory(IServiceProvider provider)
        {
            this.configuration = provider.GetService<IConfiguration>();
            this.modelBuildProvider = provider.GetService<ISecurityModelBuildProvider>();
            this.provider = provider;
        }

        public SSODbContext CreateDbContext(bool ignoreFilters = false)
        {
            if(ignoreFilters)
            {
                var authInfo = provider.GetService<ISSOAuthInfo>();
                return new IdentitySSODbContext(authInfo, modelBuildProvider);
            }

            return new SSODbContext(modelBuildProvider);
        }
    }
}
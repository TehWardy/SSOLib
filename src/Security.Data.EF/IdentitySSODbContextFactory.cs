using System;
using Microsoft.Extensions.Configuration;
using Security.Data.EF.Interfaces;
using Security.Objects;

namespace Security.Data.EF
{
    public class IdentitySSODbContextFactory : IIdentitySSODbContextFactory
    {
        private readonly IConfiguration configuration;
        private readonly ISSOAuthInfo authInfo;
        private readonly ISecurityModelBuildProvider modelBuildProvider;

        public IdentitySSODbContextFactory(IConfiguration configuration, ISSOAuthInfo authInfo, ISecurityModelBuildProvider modelBuildProvider)
        {
            this.configuration = configuration;
            this.authInfo = authInfo;
            this.modelBuildProvider = modelBuildProvider;
        }

        public IdentitySSODbContext CreateDbContext()
            => new(configuration, authInfo, modelBuildProvider);
    }
}


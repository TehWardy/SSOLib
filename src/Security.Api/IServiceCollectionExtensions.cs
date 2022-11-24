using Security.Api.DTOs;
using Security.Data;
using Security.Data.Brokers.Authentication;
using Security.Data.Brokers.Encryption;
using Security.Data.Brokers.Requests;
using Security.Data.Brokers.Serialization;
using Security.Data.Brokers.Storage;
using Security.Data.Brokers.Storage.Interfaces;
using Security.Data.EF;
using Security.Data.EF.Interfaces;
using Security.Data.Interfaces;
using Security.Objects;
using Security.Services.Foundation;
using Security.Services.Foundation.Interfaces;
using Security.Services.Processing;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Orchestration;
using Security.Services.Services.Orchestration.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using Security.UserManager.Services.Foundation;
using Security.UserManager.Services.Processing;

namespace Security.UserManager
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSecurity(this IServiceCollection services, IConfiguration configuration, Action<SecurityBuilderOptions> dbSetupAction)
        {
            var builderOptions = new SecurityBuilderOptions(services, configuration);
            dbSetupAction(builderOptions);
            builderOptions.Build();

            services.AddDbContextPool<SSODbContext>(opt => { });
            services.AddDbContextFactory<SSODbContext>();
            services.AddTransient<ISSODbContextFactory, SSODbContextFactory>();
            services.AddTransient<IIdentitySSODbContextFactory, IdentitySSODbContextFactory>();
            services.AddTransient<ISSOAuthInfo>((IServiceProvider provider)
                => provider.GetService<ISSOAuthInfoOrchestrationService>().GetSSOAuthInfo()
            );
            services.AddBrokers();
            services.AddFoundations();
            services.AddProcessings();
            services.AddOrchestrations();

            services.AddAspNet();
        }

        public static void AddBrokers(this IServiceCollection services)
        {
            services.AddTransient<IHttpRequestBroker, HttpRequestBroker>();
            services.AddTransient<ISessionBroker, SessionBroker>();
            services.AddTransient<ISSOPrivilegeBroker, SSOPrivilegeBroker>();
            services.AddTransient<ISSORoleBroker, SSORoleBroker>();
            services.AddTransient<ISSOUserBroker, SSOUserBroker>();
            services.AddTransient<ISSOUserRoleBroker, SSOUserRoleBroker>();
            services.AddTransient<ITenantBroker, TenantBroker>();
            services.AddTransient<ITenantAnalysisBroker, TenantAnalysisBroker>();
            services.AddTransient<ITokenBroker, TokenBroker>();
            services.AddTransient<IUserEventBroker, UserEventBroker>();

            services.AddTransient<IIdentityBroker, IdentityBroker>();
            services.AddTransient<ISerializationBroker, SerializationBroker>();
        }

        public static void AddFoundations(this IServiceCollection services)
        {
            services.AddTransient<ISSOUserService, SSOUserService>();
            services.AddTransient<ISSOPrivilegeService, SSOPrivilegeService>();
            services.AddTransient<ISSOUserRoleService, SSOUserRoleService>();
            services.AddTransient<ISSORoleService, SSORoleService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ISessionService, SessionService>();
        }

        public static void AddProcessings(this IServiceCollection services)
        {
            services.AddTransient<ISSOUserProcessingService, SSOUserProcessingService>();
            services.AddTransient<ISSOPrivilegeProcessingService, SSOPrivilegeProcessingService>();
            services.AddTransient<ISSOUserRoleProcessingService, SSOUserRoleProcessingService>();
            services.AddTransient<ISSORoleProcessingService, SSORoleProcessingService>();
            services.AddTransient<ITokenProcessingService, TokenProcessingService>();
            services.AddTransient<ISessionProcessingService, SessionProcessingService>();
        }

        public static void AddOrchestrations(this IServiceCollection services)
        {
            services.AddTransient<ISSOUserOrchestrationService, SSOUserOrchestrationService>();
            services.AddTransient<ISSOAuthInfoOrchestrationService, SSOAuthInfoOrchestrationService>();
            services.AddTransient<IAuthenticationOrchestrationService, AuthenticationOrchestrationService>();
        }

        public static void AddAspNet(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient(ctx => ctx.GetService<IHttpContextAccessor>()?.HttpContext);
            services.AddTransient(ctx => ctx.GetService<HttpContext>()?.Request);
            services.AddTransient(ctx => ctx.GetService<HttpContext>()?.Session);
            services.AddSession();
        }
    }
}

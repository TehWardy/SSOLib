using Microsoft.AspNetCore.Http;
using Security.Data.Brokers.Requests;
using Security.Objects;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;
using System.Text;

namespace Security.UserManager.Services.Foundation
{
    public class SSOAuthInfoOrchestrationService : ISSOAuthInfoOrchestrationService
    {
        readonly ISessionProcessingService sessionService;
        readonly ISSOUserProcessingService userService;
        private readonly ITokenProcessingService tokenService;
        private readonly IHttpRequestBroker httpRequestBroker;

        public SSOAuthInfoOrchestrationService(
            ISessionProcessingService sessionService,
            ISSOUserProcessingService userService,
            ITokenProcessingService tokenService,
            IHttpRequestBroker httpRequestBroker)
        {
            this.sessionService = sessionService;
            this.userService = userService;
            this.tokenService = tokenService;
            this.httpRequestBroker = httpRequestBroker;
        }

        public ISSOAuthInfo GetSSOAuthInfo()
        {
            if (httpRequestBroker.HasHeader("Authorization"))
                return GetFromAuthenticationHeader();

            var sessionAuthInfo = GetFromSession();

            if (sessionAuthInfo != null)
                return sessionAuthInfo;

            return new SSOAuthInfo { SSOUserId = "Guest" };
        }

        private ISSOAuthInfo GetFromAuthenticationHeader()
        {
            string authHeaderValue = httpRequestBroker.Header("Authorization");

            var bearerAuthInfo = GetBearerAuthentication(authHeaderValue);

            if (bearerAuthInfo != null)
                return bearerAuthInfo;

            try
            {
                var basicAuthInfo = GetBasicAuthentication(authHeaderValue);

                if (basicAuthInfo != null)
                    return basicAuthInfo;
            }
            catch (SecurityException)
            {
                return new SSOAuthInfo { SSOUserId = "Guest" };
            }

            return null;
        }

        ISSOAuthInfo GetFromSession()
        {
            var user = sessionService.GetUser();

            if (user == null)
                return null;

            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        ISSOAuthInfo GetBearerAuthentication(string authHeaderValue)
        {
            var tokenId = GetBearerToken(authHeaderValue);

            if (tokenId == null)
                return null;

            var token = tokenService.GetTokenById(tokenId);

            if (token == null)
                return null;

            var user = userService.FindById(token.UserName);

            if (user == null)
                return null;

            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        ISSOAuthInfo GetBasicAuthentication(string authHeaderValue)
        {
            if (authHeaderValue.ToLowerInvariant().StartsWith("basic"))
                return AuthenticateBasicAuth(authHeaderValue);

            return null;
        }

        ISSOAuthInfo AuthenticateBasicAuth(string auth)
        {
            (string username, string password) = ParseBasicAuthDetails(auth);
            var user = userService.FindByUserAndPassword(username, password);
            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        static (string, string) ParseBasicAuthDetails(string auth)
        {
            string base64AuthString = auth[6..];
            byte[] authBytes = Convert.FromBase64String(base64AuthString);
            string authString = Encoding.UTF8.GetString(authBytes);
            return (
                authString.Contains('&')
                    ? authString.Split("&")[0]
                    : authString.Split(":")[0],
                authString.Contains('&')
                    ? authString.Split("&")[1]
                    : authString.Split(":")[1]
            );
        }

        static string GetBearerToken(string auth)
        {
            if (!auth.ToLowerInvariant().StartsWith("bearer"))
                return null;

            return auth.Split(" ").LastOrDefault();
        }
    }
}
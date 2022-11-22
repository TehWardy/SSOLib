using Microsoft.AspNetCore.Http;
using Security.Objects;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Security;
using System.Text;

namespace Security.UserManager.Services.Foundation
{
    public class SSOAuthInfoOrchestrationService : ISSOAuthInfoOrchestrationService
    {
        readonly HttpRequest request;
        readonly ISessionProcessingService sessionService;
        readonly ISSOUserProcessingService userService;

        public SSOAuthInfoOrchestrationService(
            HttpRequest request,
            ISessionProcessingService sessionService,
            ISSOUserProcessingService userService)
        {
            this.request = request;
            this.sessionService = sessionService;
            this.userService = userService;
        }

        public ISSOAuthInfo GetSSOAuthInfo()
        {
            var bearerAuthInfo = GetBearerAuthentication();

            if (bearerAuthInfo != null)
                return bearerAuthInfo;

            var sessionAuthInfo = GetFromSession();

            if (sessionAuthInfo != null)
                return sessionAuthInfo;

            try
            {
                var basicAuthInfo = GetBasicAuthentication();

                if (basicAuthInfo != null)
                    return basicAuthInfo;
            }

            catch(SecurityException)
            {
                return new SSOAuthInfo { SSOUserId = "Guest" };
            }

            return new SSOAuthInfo { SSOUserId = "Guest" };
        }

        ISSOAuthInfo GetFromSession()
        {
            var user = sessionService.GetUser();

            if (user == null)
                return null;

            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        ISSOAuthInfo GetBearerAuthentication()
        {
            var tokenId = GetBearerToken();

            if (tokenId == null)
                return null;

            var user = userService.FindByTokenId(tokenId);

            if (user == null)
                return null;

            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        ISSOAuthInfo GetBasicAuthentication()
        {
            if (request != null && request.Headers.ContainsKey("Authorization"))
            {
                string auth = request.Headers["Authorization"].ToString();

                if (auth.ToLowerInvariant().StartsWith("basic"))
                    return AuthenticateBasicAuth(auth);
            }

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

        string GetBearerToken()
        {
            if (request == null || !request.Headers.ContainsKey("Authorization"))
                return null;

            string auth = request.Headers["Authorization"].ToString();

            if (!auth.ToLowerInvariant().StartsWith("bearer"))
                return null;

            return auth.Split(" ").LastOrDefault();
        }
    }
}
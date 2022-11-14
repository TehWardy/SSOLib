using Microsoft.AspNetCore.Http;
using Security.Objects;
using Security.Services.Services.Foundation.Interfaces;
using Security.Services.Services.Processing.Interfaces;
using System.Text;

namespace Security.UserManager.Services.Foundation
{
    public class SSOAuthInfoProcessingService : ISSOAuthInfoProcessingService
    {
        readonly HttpRequest request;
        readonly ISessionService sessionService;
        readonly ISSOUserService userService;

        public SSOAuthInfoProcessingService(
            HttpRequest request,
            ISessionService sessionService,
            ISSOUserService userService)
        {
            this.request = request;
            this.sessionService = sessionService;
            this.userService = userService;
        }

        public ISSOAuthInfo GetSSOAuthInfo()
        {
            var result = GetFromSession() ?? GetBearerAuthentication();
            //result ??= GetBasicAuthentication();
            result ??= new SSOAuthInfo { SSOUserId = "Guest" };
            return result;
        }

        ISSOAuthInfo GetFromSession()
        {
            var user = sessionService.GetUser();

            return user != null
                ? new SSOAuthInfo { SSOUserId = user.Id }
                : null;
        }

        ISSOAuthInfo GetBearerAuthentication()
        {
            var tokenId = GetBearerToken();

            if (tokenId == null)
                return null;

            var user = userService
                .GetAllSSOUsers()
                .FirstOrDefault(u => u.Tokens.Any(t => t.Id == tokenId));

            return new SSOAuthInfo { SSOUserId = user.Id };
        }

        /*
         * This may benefit from being a middleware function as it requires
         * an extra layer of processing
         * 
        ISSOAuthInfo GetBasicAuthentication()
        {
            if (request != null && request.Headers.ContainsKey("Authorization"))
            {
                string auth = request.Headers["Authorization"].ToString();

                if (auth.ToLowerInvariant().StartsWith("basic"))
                    return AuthenticateBasicAuth(auth).Result;
            }

            return null;
        }

        async Task<ISSOAuthInfo> AuthenticateBasicAuth(string auth)
        {
            (string username, string password) = ParseBasicAuthDetails(auth);
            var token = await userService.Login(username, password);
            var user = userService.GetUserByTokenId(token.Id);
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
                    : authString.Split(":")[1],
                authString.Contains('&')
                    ? authString.Split("&")[1]
                    : authString.Split(":")[1]
            );
        }
        */

        string GetBearerToken()
        {
            if (request == null || !request.Headers.ContainsKey("Authorization"))
                return null;

            string auth = request.Headers["Authorization"].ToString()
                    ??
                request.HttpContext.Session.GetString("token");

            if (!auth.ToLowerInvariant().StartsWith("bearer"))
                return null;

            return auth.ToLower()[(auth.ToLower().IndexOf(" ") + 1)..];
        }
    }
}
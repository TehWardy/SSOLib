using Microsoft.AspNetCore.Http;

namespace Security.Data.Brokers.Requests
{
    public class HttpRequestBroker : IHttpRequestBroker
	{
        private readonly HttpRequest request;

        public HttpRequestBroker(HttpRequest request) =>
            this.request = request;

        public bool HasHeader(string headerValue) => 
            request?.Headers.ContainsKey(headerValue) ?? false;

        public string Header(string key) => 
            request?.Headers.ContainsKey(key) ?? false
                ? request?.Headers[key].ToString() 
                : null;
    }
}
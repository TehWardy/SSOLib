using System;
using Microsoft.AspNetCore.Http;

namespace Security.Data.Brokers.Requests
{
	public class HttpRequestBroker : IHttpRequestBroker
	{
        private readonly HttpRequest request;

        public HttpRequestBroker(HttpRequest request)
		{
            this.request = request;
        }

        public bool HasHeader(string headerValue)
            => request.Headers.ContainsKey(headerValue);

        public string Header(string key)
            => request.Headers[key].ToString();
    }
}


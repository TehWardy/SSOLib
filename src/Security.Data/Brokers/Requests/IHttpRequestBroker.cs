using System;
namespace Security.Data.Brokers.Requests
{
	public interface IHttpRequestBroker
	{
        bool HasHeader(string headerValue);
        string Header(string key);
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Security.Api.OData.Responses
{
    public class RawResult : IActionResult
    {
        private readonly string response;

        public RawResult(string response) => this.response = response;

        public Task ExecuteResultAsync(ActionContext context) => Task.FromResult(new HttpResponseMessage { Content = new StringContent(response), StatusCode = System.Net.HttpStatusCode.OK });
    }
}
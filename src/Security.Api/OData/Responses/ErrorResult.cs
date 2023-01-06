using Microsoft.AspNetCore.Mvc;
using SharedObjects.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.Api.OData.Responses
{
    public class ErrorResult : IActionResult
    {
        private readonly string message;
        private readonly Exception ex;

        public ErrorResult(Exception ex) => this.ex = ex;

        public ErrorResult(string message) => this.message = message;

        public Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponseMessage response = new(HttpStatusCode.NotAcceptable);
            if (ex != null)
                response.Content =
#if DEBUG
                    new StringContent(new { message = ex.Message, trace = ex.StackTrace }.ToJson());
#else
                    new StringContent(new { message = ex.Message}.ToJson());
#endif
            else
            {
                response.Content = new StringContent(message);
            }

            return Task.FromResult(response);
        }
    }
}

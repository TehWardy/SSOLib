using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedObjects.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Security.Api.OData.Responses
{
    public class ValidationFailureResult : IActionResult
    {
        private readonly ValidationResult resultInfo;

        public ValidationFailureResult(ValidationResult resultInfo) => this.resultInfo = resultInfo;

        public Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponseMessage response = new(HttpStatusCode.NotAcceptable)
            {
                Content = new StringContent(JsonConvert.SerializeObject(resultInfo, ObjectExtensions.JSONSettings))
            };
            return Task.FromResult(response);
        }
    }
}

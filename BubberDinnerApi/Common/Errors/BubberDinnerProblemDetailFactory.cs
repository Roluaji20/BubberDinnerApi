using BubberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BubberDinner.Api.Common.Errors
{
    public class BubberDinnerProblemDetailFactory : ProblemDetailsFactory
    {

        private readonly ApiBehaviorOptions _options;

        public BubberDinnerProblemDetailFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {

            statusCode ??= 500;

            var problemDetails = new ProblemDetails
            {

                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            if(title != null) 
            {
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {

            var problemDetails = new ProblemDetails
            {

                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            var validationProblemDetails = new ValidationProblemDetails
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            return validationProblemDetails;
           // throw new NotImplementedException();
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {

            problemDetails.Status ??= statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode,out var clientErrorData)) 
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

            if (traceId != null)
            {
                problemDetails.Extensions["traceId"]= traceId;
            }

            var erros = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;

            if (erros is not null)
            {
                 problemDetails.Extensions.Add("errorsCode", erros.Select(e=>e.Code));

            }
            
        }

       
    }
}

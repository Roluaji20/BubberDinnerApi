using BubberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BubberDinner.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {

        protected IActionResult Problem(List<Error> errors)
        {

            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }
            HttpContext.Items[HttpContextItemKeys.Errors] = errors;


            return Problem(errors[0]);
        }

        private IActionResult Problem(Error error)
        {
            var statuscode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError

            };

            return Problem(statusCode: statuscode, title: error.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelstateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelstateDictionary.AddModelError(
                    error.Code,
                    error.Description
                    );
            }
            return ValidationProblem(modelstateDictionary);
        }
    }
}

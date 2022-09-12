using BubberDinner.Aplication.Authentication.Commands.Queries.Login;
using BubberDinner.Aplication.Authentication.Commands.Register;
using BubberDinner.Contracts.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BubberDinner.Domain.Common.Errors;
using BubberDinner.Aplication.Authentication.Common;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace BubberDinner.Api.Controllers
{
   
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
                
            ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

            return authResult.Match(
                    authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                    errors => Problem(errors)
                ); ;
         
        }

       
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var query = _mapper.Map<LoginQuery>(request);   
            var authResult = await _mediator.Send(query);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized,
                                title: authResult.FirstError.Description);
            }

            return authResult.Match(
                 authResult=> Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                 errors => Problem(errors));

           
        }
    }
}

using BubberDinner.Aplication.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BubberDinner.Aplication.Authentication.Commands.Queries.Login
{
    public record LoginQuery(    string Email,
                                 string Password) : IRequest<ErrorOr<AuthenticationResult>>;
}

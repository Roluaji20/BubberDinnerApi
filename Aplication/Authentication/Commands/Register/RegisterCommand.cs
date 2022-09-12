﻿using BubberDinner.Aplication.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BubberDinner.Aplication.Authentication.Commands.Register
{
    public record RegisterCommand(string FirstName,
                                  string LastName,
                                  string Email,
                                  string Password):IRequest<ErrorOr<AuthenticationResult>>;
}

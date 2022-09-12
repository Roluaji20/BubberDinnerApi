using BubberDinner.Aplication.Common.Authentication;
using BubberDinner.Aplication.Persistence;
using ErrorOr;
using MediatR;
using BubberDinner.Domain.Common.Errors;
using BubberDinner.Domain.Entities;
using BubberDinner.Aplication.Authentication.Common;

namespace BubberDinner.Aplication.Authentication.Commands.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {

        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator,
                                 IUserRepository userRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            //1. Validate the user exists
            if (_userRepository.GetUserByEmail(query.Email) is not User user)
            {
                // throw new Exception("User with given Email doesn't exist");
                return Errors.Authentication.InvalidCredentials;
            }

            //2 Validate Password correct
            if (user.Password != query.Password)
            {
                //throw new Exception("Invalid Password");
                return new[] { Errors.Authentication.InvalidCredentials };

            }

            //3. Create JWT
            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}

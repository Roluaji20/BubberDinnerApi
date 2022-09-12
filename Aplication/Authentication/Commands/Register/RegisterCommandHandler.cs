using BubberDinner.Aplication.Common.Authentication;
using BubberDinner.Aplication.Persistence;
using BubberDinner.Domain.Entities;
using BubberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BubberDinner.Aplication.Authentication.Common;

namespace BubberDinner.Aplication.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator,
                                      IUserRepository userRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }
        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            //1. vaidate the user doesn't exist
            if (_userRepository.GetUserByEmail(command.Email) is not null)
            {
              return Errors.User.DuplicateEmail;
              }

            //2. create a new user(generate unique id)
            var user = new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password
            };

            _userRepository.Add(user);


            //3. generate token
            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}

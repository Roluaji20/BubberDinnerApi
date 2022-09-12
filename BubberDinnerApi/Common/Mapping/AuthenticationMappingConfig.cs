using BubberDinner.Aplication.Authentication.Commands.Queries.Login;
using BubberDinner.Aplication.Authentication.Commands.Register;
using BubberDinner.Aplication.Authentication.Common;
using BubberDinner.Contracts.Authentication;
using Mapster;

namespace BubberDinner.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest,RegisterCommand>();
            config.NewConfig<LoginRequest, LoginQuery>();

            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                 .Map(dest => dest.Token, src => src.Token)
                 .Map(dest => dest, src => src.User);
        }
    }
}

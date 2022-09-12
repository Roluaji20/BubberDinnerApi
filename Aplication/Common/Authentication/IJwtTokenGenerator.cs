using BubberDinner.Domain.Entities;

namespace BubberDinner.Aplication.Common.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}

using BubberDinner.Domain.Entities;

namespace BubberDinner.Aplication.Persistence
{
    public interface IUserRepository
    {

        User? GetUserByEmail(string Email);

        void Add(User user); 

    }
}

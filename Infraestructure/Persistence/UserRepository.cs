using BubberDinner.Aplication.Persistence;
using BubberDinner.Domain.Entities;

namespace BubberDinner.Infraestructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        
        private static readonly List<User> _users = new List<User>();
        public void Add(User user)
        {
            _users.Add(user);
        }

        public User? GetUserByEmail(string Email)
        {
           return _users.SingleOrDefault(x => x.Email == Email);
            
        }
    }
}

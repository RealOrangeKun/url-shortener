

using UrlShortener.Models;
using UrlShortener.Repositories;

namespace UrlShortener.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        public async Task CreateUserAsync(User user)
        {
            await repository.AddUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await repository.DeleteUserAsync(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await repository.GetUserByIdAsync(id);
        }

        public User GetUserByUserNameAsync(string userName)
        {
            return repository.GetUserByUserNameAsync(userName);
        }

        public async Task UpdateUserAsync(User user)
        {
            await repository.UpdateUserAsync(user);
        }
    }
}
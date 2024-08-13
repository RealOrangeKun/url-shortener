using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUserService
    {
        User GetUserByUserNameAsync(string userName);
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

    }
}
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUserNameAsync(string userName);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UserRepository(UrlShortenerDbContext context) : IUserRepository
    {
        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            User user = await context.Users.FindAsync(id) ?? throw new Exception("User not found");

            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id) ?? throw new Exception("User not found");
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await context.Users.FindAsync(userName) ?? throw new Exception("User not found");
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);

            await context.SaveChangesAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UserRepository(UrlShortenerDbContext context, IPasswordHasher<User> PasswordHasher) : IUserRepository
    {
        public async Task AddUserAsync(User user)
        {
            user.Password = PasswordHasher.HashPassword(user, user.Password);
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

        public User GetUserByUserNameAsync(string userName)
        {
            return context.Users.SingleOrDefault(u => u.Name == userName) ?? throw new Exception("User not found");
        }

        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);

            await context.SaveChangesAsync();
        }
    }
}
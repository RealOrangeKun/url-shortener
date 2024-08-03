using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class UrlRepository(UrlShortenerDbContext context) : IUrlRepository
    {
        public async Task AddUrlAsync(Url url)
        {
            await context.Urls.AddAsync(url);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUrlAsync(int id)
        {
            Url? url = context.Urls.Find(id) ?? throw new Exception("Url not found");
            context.Urls.Remove(url);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Url>> GetAllUrlsAsync()
        {
            return await context.Urls.ToListAsync();
        }

        public async Task<Url> GetUrlAsync(int id)
        {
            return await context.Urls.FindAsync(id) ?? throw new Exception("Url not found");
        }

        public async Task UpdateUrlAsync(Url url)
        {
            context.Urls.Update(url);
            await context.SaveChangesAsync();
        }
    }
}
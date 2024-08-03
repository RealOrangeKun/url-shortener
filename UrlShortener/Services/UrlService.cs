using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Models;
using UrlShortener.Repositories;

namespace UrlShortener.Services
{
    public class UrlService(IUrlRepository repository) : IUrlService
    {
        public async Task CreateUrlAsync(Url url)
        {
            await repository.AddUrlAsync(url);
        }

        public async Task DeleteUrlAsync(int id)
        {
            await repository.DeleteUrlAsync(id);
        }

        public async Task<IEnumerable<Url>> GetAllUrlsAsync()
        {
            return await repository.GetAllUrlsAsync();
        }

        public async Task<Url> GetUrlByIdAsync(int id)
        {
            return await repository.GetUrlAsync(id);
        }

        public async Task UpdateUrlAsync(Url url)
        {
            await repository.UpdateUrlAsync(url);
        }
    }
}
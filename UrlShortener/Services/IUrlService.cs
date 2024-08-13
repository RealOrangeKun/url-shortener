using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<Url> GetUrlByIdAsync(int id);
        Task<IEnumerable<Url>> GetAllUrlsAsync(int userId);
        Task CreateUrlAsync(Url url);
        Task UpdateUrlAsync(Url url);
        Task DeleteUrlAsync(int id);

        Task<Url> GetByShortUrlAsync(string shortUrl);
    }
}
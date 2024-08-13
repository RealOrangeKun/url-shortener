using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUrlRepository
    {
        Task<Url> GetUrlAsync(int id);
        Task<IEnumerable<Url>> GetAllUrlsAsync(int userId);
        Task AddUrlAsync(Url url);
        Task UpdateUrlAsync(Url url);
        Task DeleteUrlAsync(int id);
        Task<Url> GetByShortUrlAsync(string shortUrl);
    }
}

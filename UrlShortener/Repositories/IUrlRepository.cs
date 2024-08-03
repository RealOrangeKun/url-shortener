using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public interface IUrlRepository
    {
        Task<Url> GetUrlAsync(int id);
        Task<IEnumerable<Url>> GetAllUrlsAsync();
        Task AddUrlAsync(Url url);
        Task UpdateUrlAsync(Url url);
        Task DeleteUrlAsync(int id);
    }
}
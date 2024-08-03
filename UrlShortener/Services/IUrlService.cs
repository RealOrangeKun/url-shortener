using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        Task<Url> GetUrlByIdAsync(int id);
        Task<IEnumerable<Url>> GetAllUrlsAsync();
        Task CreateUrlAsync(Url url);
        Task UpdateUrlAsync(Url url);
        Task DeleteUrlAsync(int id);
    }
}
using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.Utilities
{
    public class UrlShortenerUtility : IUrlShortener
    {
        public Task<string> ShortenUrl(string url)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(url));
            StringBuilder stringBuilder = new();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}
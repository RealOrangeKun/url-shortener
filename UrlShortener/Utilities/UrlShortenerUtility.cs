using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.Utilities
{
    public class UrlShortenerUtility : IUrlShortener
    {
        public Task<string> ShortenUrl(string url)
        {
            if (!IsValidUrl(url)) throw new ArgumentException("Invalid Url");
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(url));
            StringBuilder stringBuilder = new();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return Task.FromResult(stringBuilder.ToString());
        }
        private static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
            {
                // Check for valid schemes
                bool isValidScheme = uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;

                // Check for a valid host
                bool isValidHost = Uri.CheckHostName(uriResult.Host) != UriHostNameType.Unknown;

                return isValidScheme && isValidHost;
            }

            return false;
        }
    }
}
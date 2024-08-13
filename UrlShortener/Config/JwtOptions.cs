

namespace UrlShortener.Config
{
    public class JwtOptions
    {
        public required string SigningKey { get; set; }
        public int LifetimeInMinutes { get; set; }
    }
}
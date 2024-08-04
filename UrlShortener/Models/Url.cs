namespace UrlShortener.Models
{
    public class Url
    {
        public required int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortUrl { get; set; }
        public required int UserId { get; set; }
    }
}
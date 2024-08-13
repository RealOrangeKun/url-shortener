
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class Token
    {
        [Key]
        public required string Value { get; set; }

        public int Subject { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Token() { }

        public Token(string value, int subject, DateTime expiration)
        {
            Value = value;
            Subject = subject;
            ExpirationDate = expiration;
        }

    }
}
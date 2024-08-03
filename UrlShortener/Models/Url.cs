using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class Url
    {
        public required int Id { get; set; }
        public required string OriginalUrl { get; set; }
        public required string ShortUrl { get; set; }
    }
}
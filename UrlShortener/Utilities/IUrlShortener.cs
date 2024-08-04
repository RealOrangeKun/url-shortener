using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Utilities
{
    public interface IUrlShortener
    {
        public Task<string> ShortenUrl(string url);
    }
}
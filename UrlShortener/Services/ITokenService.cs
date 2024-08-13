using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(int userId);
        string GenerateRefreshToken(int userId);
        bool ValidateRefreshToken(string token);
        bool ValidateAccessToken(string token);
    }
}
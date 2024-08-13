using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Repositories;

namespace UrlShortener.Services
{
    public class TokenService(ITokenRepository tokenRepository) : ITokenService
    {
        public string GenerateAccessToken(int userId)
        {
            return tokenRepository.GenerateAccessToken(userId);
        }

        public string GenerateRefreshToken(int userId)
        {
            return tokenRepository.GenerateRefreshToken(userId);
        }

        public bool ValidateAccessToken(string token)
        {
            return tokenRepository.ValidateToken(token);
        }

        public bool ValidateRefreshToken(string token)
        {
            return tokenRepository.ValidateRefreshToken(token);
        }
    }
}
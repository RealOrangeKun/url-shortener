using System.IdentityModel.Tokens.Jwt;

namespace UrlShortener.Repositories
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(int userId);

        string GenerateRefreshToken(int userId);

        bool ValidateToken(string token);

        bool ValidateRefreshToken(string token);

    }
}
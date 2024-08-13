using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Config;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Repositories
{
    public class TokenRepository(UrlShortenerDbContext context, IOptions<JwtOptions> tokenSettings) : ITokenRepository
    {
        public string GenerateAccessToken(int userId)
        {
            JwtSecurityToken token = GenerateToken(userId);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken(int userId)
        {
            JwtSecurityToken token = GenerateToken(userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            string valueToken = tokenHandler.WriteToken(token).ToString();
            int subject = int.Parse(tokenHandler.ReadJwtToken(valueToken).Claims.First(claim => claim.Type == "sub").Value);
            DateTime expiration = tokenHandler.ReadJwtToken(valueToken).ValidTo;
            var refreshToken = new Token
            {
                Value = valueToken,
                Subject = subject,
                ExpirationDate = expiration
            };
            context.Tokens.Add(refreshToken);
            context.SaveChanges();
            return refreshToken.Value;
        }


        private JwtSecurityToken GenerateToken(int userID)
        {
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var signingKey = tokenSettings.Value.SigningKey.ToString();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenSettings.Value.LifetimeInMinutes),
                signingCredentials: creds
            );
            return token;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSettings.Value.SigningKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ValidateRefreshToken(string token)
        {
            return ValidateToken(token) && context.Tokens.Any(t => t.Value == token);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Config;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController(IUserService userService, ITokenService tokenService, IPasswordHasher<User> passwordHasher) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(User user)
        {
            try
            {
                User loginUser = userService.GetUserByUserNameAsync(user.Name);

                if (loginUser == null) return NotFound("User not found");

                if (!VerifyPassword(loginUser.Password, user.Password)) return Unauthorized();

                string accessToken = tokenService.GenerateAccessToken(loginUser.Id);
                string refreshToken = tokenService.GenerateRefreshToken(loginUser.Id);

                HttpContext.Response.Cookies.Append("jwtRefresh", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                });

                return Ok(new { AccessToken = accessToken });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var user = new User();
            var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
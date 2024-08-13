using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController(IUserService userService, ITokenService tokenService, IPasswordHasher<User> passwordHasher, ILogger<AuthenticationController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public ActionResult Login(User user)
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
                logger.LogInformation("User :{userId} has logged in", loginUser.Id);
                return Ok(new { AccessToken = accessToken });
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An ArgumentException occurred while logging in: {userId}", user.Name);
                return BadRequest(ex.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception occurred while logging in: {userId}", user.Name);
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
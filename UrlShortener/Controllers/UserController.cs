using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<ActionResult<User>> GetById()
        {
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int id = int.Parse(userClaim?.Value ?? throw new ArgumentException("User not found"));
                User user = await userService.GetUserByIdAsync(id);
                logger.LogInformation("User :{userId} has requested their profile", id);
                return user == null ? NotFound("User not found") : Ok(user);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An ArgumentException occurred while getting the user: {userId}", User.Identity.Name);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception occurred while getting the user: {userId}", User.Identity.Name);
                return StatusCode(500, ex.ToString());
            }

        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(User user)
        {
            try
            {
                await userService.CreateUserAsync(user);
                logger.LogInformation("User :{userId} has registered", user.Id);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (ArgumentException e)
            {
                logger.LogError(e, "An ArgumentException occurred while registering the user: {userId}", user.Id);
                logger.LogDebug(e, "An ArgumentException occurred while registering a user {error}", e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError("An exception occurred while registering the user: {userId}", user.Id);
                logger.LogDebug(e, "An exception occurred while registering a user {error}", e.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("refresh")]
        public ActionResult RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["jwtRefresh"] ?? throw new UnauthorizedAccessException("Not authorized");
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userClaim?.Value ?? throw new UnauthorizedAccessException("Not authorized"));
                if (!tokenService.ValidateRefreshToken(refreshToken)) throw new UnauthorizedAccessException("Refresh token not valid");
                string accessToken = tokenService.GenerateAccessToken(userId);
                string newRefreshToken = tokenService.GenerateRefreshToken(userId);

                HttpContext.Response.Cookies.Append("jwtRefresh", newRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
                logger.LogInformation("User :{userId} has refreshed their token", userId);
                return Ok(new { AccessToken = accessToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, "An UnauthorizedAccessException occurred while refreshing the token: {userId}", User.Identity?.Name);
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An ArgumentException occurred while refreshing the token: {userId}", User.Identity?.Name);
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                logger.LogError("An exception occurred while refreshing the token: {userId}", User.Identity?.Name);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
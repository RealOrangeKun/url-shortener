using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
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
                return user == null ? NotFound("User not found") : Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
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
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {

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

                return Ok(new { AccessToken = accessToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
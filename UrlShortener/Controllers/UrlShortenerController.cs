using System.Data.Entity.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Authorize]
    [Route("")]
    public class UrlController(IUrlService urlService, IUrlShortener Shortener, IUserService userService, ILogger<UrlController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("api/url")]
        public async Task<ActionResult<string>> Shorten([FromBody] Dictionary<string, string> url)
        {
            try
            {
                if (!url.TryGetValue("url", out string? value)) throw new ArgumentException("Key 'url' not found");
                string shortUrl = await Shortener.ShortenUrl(value);
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userClaim?.Value ?? throw new UnauthorizedAccessException("Not Authorized"));

                Url urlModel = new()
                {
                    OriginalUrl = value,
                    ShortUrl = shortUrl[0..10],
                    UserId = userId,
                    Id = 0
                };
                await urlService.CreateUrlAsync(urlModel);
                logger.LogInformation("A URL has been shortened: {url}", urlModel.OriginalUrl);
                return Ok($"{Request.Scheme}://{Request.Host}/{urlModel.ShortUrl}");
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An ArgumentException occurred while shortening the URL: {url}", url["url"]);
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, "An UnauthorizedAccessException occurred while shortening the URL: {url}", url["url"]);
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                logger.LogError("An exception occurred while shortening the URL: {url}", url["url"]);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("{shortUrl}")]
        [AllowAnonymous]
        public async Task<ActionResult> Route(string shortUrl)
        {
            try
            {
                Url url = await urlService.GetByShortUrlAsync(shortUrl) ?? throw new ObjectNotFoundException("Url not found");
                logger.LogInformation("A URL has been redirected to: {url}", url.OriginalUrl);
                return Redirect(url.OriginalUrl);
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex, "An ObjectNotFoundException occurred while redirecting to the URL: {url}", shortUrl);
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                logger.LogError("An exception occurred while redirecting to the URL: {url}", shortUrl);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("api/url")]
        public async Task<ActionResult<List<Url>>> GetAll()
        {
            try
            {
                var userClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userClaim?.Value ?? throw new UnauthorizedAccessException("Not authorized"));
                var urls = await urlService.GetAllUrlsAsync(userId);
                logger.LogInformation("User :{userId} has requested their urls", userId);
                return Ok(urls);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, "An UnauthorizedAccessException occurred while getting the URL: {userId}", User.Identity?.Name);
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                logger.LogError("An exception occurred while getting the URL: {userId}", User.Identity?.Name);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
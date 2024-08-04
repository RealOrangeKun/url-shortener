using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController(IUrlService Service, IUrlShortener Shortener) : ControllerBase
    {

    }
}
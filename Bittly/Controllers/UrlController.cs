using Bittly.Dtos;
using Bittly.Models;
using Bittly.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bittly.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        // The IUrlService is injected via Dependency Injection
        public UrlController (IUrlService urlService)
        {
            _urlService = urlService;
        }
        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl([FromBody] ShortenUrlDto request)
        {

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Safely parse the ID (assuming your database uses integers)
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { message = "Invalid token or missing user identity." });
            }
            try
            {
                // Pass the optional fields down to the service
                ShortenUrlDto result = await _urlService.CreateShortUrlAsync(
                    request.Url,
                    userId,
                    request.ShortUrl,
                    request.ExpirationDate
                );

                return CreatedAtAction(nameof(GetUrlDetails), new { shortCode = result.ShortUrl }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Catches the "Alias already in use" exception and returns HTTP 409 Conflict
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Catches the "Expiration date must be in the future" exception and returns HTTP 400 Bad Request
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while generating the short link.");
            }
        }

        [HttpGet("{shortCode}/details")]
        public IActionResult GetUrlDetails(string shortCode)
        {
            return Ok();
        }
    }
}

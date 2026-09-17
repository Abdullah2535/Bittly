using Bittly.Configurations;
using Bittly.Dtos;
using Bittly.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bittly.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ApiControllerBase
    {
        private readonly IUrlService _urlService;
        private readonly IValidator<ShortenUrlDto> _validator;

        public UrlController(IUrlService urlService, IValidator<ShortenUrlDto> validator)
        {
            _urlService = urlService;
            _validator = validator;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> CreateShortUrl([FromBody] ShortenUrlDto request, CancellationToken ct)
        {
            var validationResult = await _validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                var firstError = validationResult.Errors.First();
                var validationError = Error.Validation("Request.ValidationFailed", firstError.ErrorMessage);

                return HandleFailure(validationError); 
            }
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { message = "Invalid token or missing user identity." });
            }

         
            Result<ShortenUrlDto> result = await _urlService.CreateShortUrlAsync(
                request.Url,
                userId,
                request.ShortUrl,
                request.ExpirationDate
            );

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return CreatedAtAction(nameof(GetUrlDetails), new { shortCode = result.Value!.ShortUrl }, result.Value);
        }

        [HttpGet("{shortCode}/details")]
        public async Task<IActionResult> GetUrlDetails(string shortCode)
        {
            var result = await _urlService.GetUrlDetailsAsync(shortCode);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{shortCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> RedirectToLongUrl(string shortCode)
        {
            var result = await _urlService.GetValidLongUrlAsync(shortCode);

            if (result.IsFailure)
            {
                // return the 404 or 400 smoothly
                return HandleFailure(result.Error);
            }

            return Redirect(result.Value!);
        }

        [HttpGet("{id}/links")]
        public async Task<IActionResult> UserLinks(int id)
        {
            var result = await _urlService.GetUserUrlsAsync(id);

            if (result.IsFailure)
            {
                return HandleFailure(result.Error);
            }

            return Ok(result.Value);
        }

      
    }
}
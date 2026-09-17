using Bittly.Configurations;
using Bittly.Dtos;

namespace Bittly.Services
{
    public interface IUrlService
    {
        // Returns the created DTO on success, or an error (e.g., "Alias already in use") on failure
        Task<Result<ShortenUrlDto>> CreateShortUrlAsync(string longUrl, int userId, string? customAlias = null, string? customExpiration = null);

        // Returns the long URL on success, or an error (e.g., "Link expired" or "Not found")
        Task<Result<string>> GetValidLongUrlAsync(string shortUrl);

        // Returns details on success, or an error if the short URL doesn't exist
        Task<Result<ShortenUrlDto>> GetUrlDetailsAsync(string shortUrl);

        // Returns the list (even if empty) on success. Rarely fails, but keeps the API surface consistent.
        Task<Result<IEnumerable<ShortenUrlDto>>> GetUserUrlsAsync(int userId);
    }
}

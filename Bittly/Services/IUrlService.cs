using Bittly.Dtos;

namespace Bittly.Services
{
    public interface IUrlService
    {
        // For creating a new short link
        Task<ShortenUrlDto> CreateShortUrlAsync(string longUrl, int userId,string? customAlias = null, string? customExpiration = null);

        // For the redirect operation (returns the raw string if valid)
        Task<string> GetValidLongUrlAsync(string shortUrl);

        // For a user's dashboard to see their links
     //   Task<IEnumerable<UrlResponseDto>> GetUserUrlsAsync(int userId);
    }
}

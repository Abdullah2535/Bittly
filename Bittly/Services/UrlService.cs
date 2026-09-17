using Bittly.Configurations;
using Bittly.Dtos;
using Bittly.Infrastructure;
using Bittly.Models;
using System.Globalization;

namespace Bittly.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IShortenerService _shortenerService;

        private readonly IUnitOfWork _unitOfWork;


        public UrlService(IUrlRepository urlRepository, IShortenerService shortener, IUnitOfWork unitOfWork)
        {
            _urlRepository = urlRepository;
            _shortenerService = shortener;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ShortenUrlDto>> CreateShortUrlAsync(string longUrl, int userId, string? customAlias = null, string? customExpiration = null)
        {
            // Handle Short Code Generation or Custom Alias
            var shortCodeResult = await GetUniqueShortCodeAsync(customAlias);
            if (shortCodeResult.IsFailure)
            {
                return Result<ShortenUrlDto>.Failure(shortCodeResult.Error);
            }

            string shortCode = shortCodeResult.Value!;

            // Handle Expiration Date
            var (finalExpirationDate, formattedExpirationOutput) = ParseExpirationDate(customExpiration);

            var newUrl = new Url
            {
                LongUrl = longUrl,
                UserId = userId,
                ShortUrl = shortCode,
                ExiprationDate = finalExpirationDate 
            };

            await _urlRepository.AddAsync(newUrl);
            await _unitOfWork.SaveChangesAsync();

            return Result<ShortenUrlDto>.Success(new ShortenUrlDto
            {
                Url = newUrl.LongUrl,
                ShortUrl = newUrl.ShortUrl,
                ExpirationDate = formattedExpirationOutput
            });
        }
        public async Task<Result<string>> GetValidLongUrlAsync(string shortUrl)
        {
            var urlEntity = await _urlRepository.GetByShortUrlAsync(shortUrl);

            if (urlEntity == null)
            {
                return Result<string>.Failure(Error.NotFound("Url.NotFound", "The requested short URL does not exist."));
            }

            if (DateTime.UtcNow > urlEntity.ExiprationDate)
            {
                return Result<string>.Failure(Error.Validation("Url.Expired", "This link has expired."));
            }

            return Result<string>.Success(urlEntity.LongUrl);
        }

        public async Task<Result<ShortenUrlDto>> GetUrlDetailsAsync(string shortUrl)
        {
            var urlEntity = await _urlRepository.GetByShortUrlAsync(shortUrl);

            if (urlEntity == null)
            {
                return Result<ShortenUrlDto>.Failure(Error.NotFound("Url.NotFound", "The requested short URL does not exist."));
            }

            if (DateTime.UtcNow > urlEntity.ExiprationDate)
            {
                return Result<ShortenUrlDto>.Failure(Error.Validation("Url.Expired", "This link has expired."));
            }

            return Result<ShortenUrlDto>.Success(new ShortenUrlDto
            {
                Url = urlEntity.LongUrl,
                ShortUrl = urlEntity.ShortUrl,
                ExpirationDate = urlEntity.ExiprationDate.ToString("yyyy-MM-dd")
            });
        }

        public async Task<Result<IEnumerable<ShortenUrlDto>>> GetUserUrlsAsync(int userId)
        {
            var urls = await _urlRepository.GetUrlsByUserIdAsync(userId);

            var dtos = urls.Select(u => new ShortenUrlDto
            {
                Url = u.LongUrl,
                ShortUrl = u.ShortUrl,
                ExpirationDate = u.ExiprationDate.ToString("yyyy-MM-dd")
            });

            return Result<IEnumerable<ShortenUrlDto>>.Success(dtos);
        }


        private async Task<Result<string>> GetUniqueShortCodeAsync(string? customAlias)
        {
            // User provided a custom alias
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                var existingAlias = await _urlRepository.GetByShortUrlAsync(customAlias);
                if (existingAlias != null)
                {
                    return Result<string>.Failure(
                        Error.Conflict("Url.AliasTaken", "This custom alias is already in use. Please choose another."));
                }

                return Result<string>.Success(customAlias);
            }

            //  Generate a random short code
            const int MaxAttempts = 5;

            for (int attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                string shortCode = _shortenerService.GenerateRandomShortCode();
                var existingUrl = await _urlRepository.GetByShortUrlAsync(shortCode);

                if (existingUrl == null)
                {
                    return Result<string>.Success(shortCode);
                }
            }

            return Result<string>.Failure(
                Error.Failure("Url.GenerationFailed", "Failed to generate a unique short code after maximum attempts."));
        }

 
        private (DateTime Date, string FormattedString) ParseExpirationDate(string? validExpirationDate)
        {
            // 1. Handle Default 30-day scenario
            if (string.IsNullOrWhiteSpace(validExpirationDate))
            {
                return (DateTime.UtcNow.AddDays(30), "The Link will Last for 30 days");
            }

            // 2. Transform the guaranteed valid string into our UTC end-of-day DateTime
            var parsedDate = DateOnly.ParseExact(validExpirationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime finalDate = parsedDate.ToDateTime(new TimeOnly(23, 59, 59), DateTimeKind.Utc);

            return (finalDate, finalDate.ToString("yyyy-MM-dd"));
        }
    }
}
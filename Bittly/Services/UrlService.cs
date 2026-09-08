using Bittly.Dtos;
using Bittly.Infrastructure;
using Bittly.Models;
using System.Globalization;

namespace Bittly.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int ShortCodeLength = 6;

        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<ShortenUrlDto> CreateShortUrlAsync(string longUrl, int userId, string? customAlias = null, string? customExpiration = null)
        {
            string shortCode;

            // 1. Handle Short Code Generation or Custom Alias
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                // Business Rule: Check if the custom alias is already taken
                var existingAlias = await _urlRepository.GetByShortUrlAsync(customAlias);
                if (existingAlias != null)
                {
                    // We throw an exception so the controller can return a 409 Conflict
                    throw new InvalidOperationException("This custom alias is already in use. Please choose another.");
                }
                shortCode = customAlias;
            }
            else
            {
                // Fall back to the random generator with the collision loop
                bool isUnique = false;
                do
                {
                    shortCode = GenerateRandomShortCode();
                    var existingUrl = await _urlRepository.GetByShortUrlAsync(shortCode);
                    if (existingUrl == null)
                    {
                        isUnique = true;
                    }
                } while (!isUnique);
            }

            // 2. Handle Expiration Date
            var (finalExpirationDate, formattedExpirationOutput) = ParseExpirationDate(customExpiration);

            // 3. Save to Database
            var newUrl = new Url
            {
                LongUrl = longUrl,
                UserId = userId,
                ShortUrl = shortCode,
                ExiprationDate = finalExpirationDate
            };

            await _urlRepository.AddAsync(newUrl);
            await _urlRepository.SaveChangesAsync();

            return new ShortenUrlDto
            {
                Url = newUrl.LongUrl,
                ShortUrl = newUrl.ShortUrl,
                ExpirationDate = formattedExpirationOutput
            };
        }

        public async Task<string> GetValidLongUrlAsync(string shortUrl)
        {
            // 1. Fetch the entity
            var urlEntity = await _urlRepository.GetByShortUrlAsync(shortUrl);

            // 2. Business Logic: Does it exist?
            if (urlEntity == null)
            {
                // Throwing an exception here allows global error handling in the API
                throw new KeyNotFoundException("The requested short URL does not exist.");
            }

            // 3. Business Logic: Is it expired?
            if (DateTime.UtcNow > urlEntity.ExiprationDate)
            {
                throw new InvalidOperationException("This link has expired.");
            }

            // 4. Return just the string needed for the redirect
            return urlEntity.LongUrl;
        }

        //public async Task<IEnumerable<UrlResponseDto>> GetUserUrlsAsync(int userId)
        //{
        //    // Fetch entities
        //    var urls = await _urlRepository.GetUrlsByUserIdAsync(userId);

        //    // Map entities to DTOs using LINQ
        //    return urls.Select(u => new UrlResponseDto
        //    {
        //        LongUrl = u.LongUrl,
        //        ShortUrl = u.ShortUrl,
        //        ExpirationDate = u.ExiprationDate
        //    });
        //}

        // A private helper method to generate a random 6-character string
        private string GenerateRandomShortCode()
        {
            var randomChars = new char[ShortCodeLength];
            for (int i = 0; i < ShortCodeLength; i++)
            {
                // Select a random character from the 62 available options
                randomChars[i] = Base62Chars[Random.Shared.Next(Base62Chars.Length)];
            }
            return new string(randomChars);
        }

        private (DateTime Date, string FormattedString) ParseExpirationDate(string? expirationDate)
        {
            // Guard Clause 1: If it's empty, return the defaults immediately and exit.
            if (string.IsNullOrWhiteSpace(expirationDate))
            {
                return (DateTime.UtcNow.AddDays(30), "The Link will Last for 30 days");
            }

            // Guard Clause 2: If parsing fails, throw immediately and exit.
            if (!DateTime.TryParseExact(expirationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                throw new ArgumentException("Invalid date format. Please use exactly YYYY-MM-DD (e.g., 2026-12-31).");
            }

            // Process the valid date
            DateTime finalDate = parsedDate.Date.AddDays(1).AddTicks(-1);

            // Guard Clause 3: If it's in the past, throw immediately.
            if (finalDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("Expiration date must be in the future.");
            }

            // The end result.
            return (finalDate, finalDate.ToString("yyyy-MM-dd"));
        }
    }
}

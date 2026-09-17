using Bittly.Configurations;
using Microsoft.Extensions.Options;

namespace Bittly.Services
{
    public class ShortenerService : IShortenerService
    {
        private readonly ShortenerOptions _options;
        public ShortenerService(IOptions<ShortenerOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateRandomShortCode()
        {
            var randomChars = new char[_options.ShortCodeLength];
            for (int i = 0; i < _options.ShortCodeLength; i++)
            {
                randomChars[i] = _options.Alphabet[Random.Shared.Next(_options.Alphabet.Length)];
            }
            return new string(randomChars);
        }

    }
}

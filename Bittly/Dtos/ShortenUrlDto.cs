using System.ComponentModel.DataAnnotations;

namespace Bittly.Dtos
{
    public class ShortenUrlDto
    {
        [Required]
        public string Url { get; set; } = string.Empty;

        [StringLength(6)]
        public string ? ShortUrl { get; set; }

        public string? ExpirationDate { get; set; }

    }
}

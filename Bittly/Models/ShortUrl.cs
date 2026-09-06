namespace Bittly.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }

        public string ShortenedUrl { get; set; } = string.Empty;


        public int OriginalUrlId { get; set; }

        public OriginalUrl OriginalUrl { get; set; }

    }
}

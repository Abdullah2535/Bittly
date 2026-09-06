using System.ComponentModel.DataAnnotations;

namespace Bittly.Models
{
    public class OriginalUrl
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        public int UserId { get; set; }

        public ShortUrl ShortUrl { get; set; }

    }
}

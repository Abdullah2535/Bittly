using System.ComponentModel.DataAnnotations;

namespace Bittly.Models
{
    public class Url
    {
        public int UrlId { get; set; }

        [Required]
        [StringLength(255)]
        public string LongUrl { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string ShortUrl { get; set; } = string.Empty;

        public DateTime ExiprationDate { get; set; }


    }
}

using Microsoft.AspNetCore.Identity;

namespace Bittly.Models
{
    public class ApplicationUser : IdentityUser <int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public OriginalUrl OriginalUrl { get; set; }

        public int OriginalUrlID { get; set; }
    }
}

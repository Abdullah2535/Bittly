using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bittly.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OriginalUrl> OriginalUrls { get; set; }
        public DbSet<ShortUrl> ShortUrls { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OriginalUrl>()
                .HasOne(o => o.ShortUrl)
                .WithOne(s => s.OriginalUrl)
                .HasForeignKey<ShortUrl>(s => s.OriginalUrlId);

            builder.Entity<OriginalUrl>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId);

            base.OnModelCreating(builder);
        }

    }

}

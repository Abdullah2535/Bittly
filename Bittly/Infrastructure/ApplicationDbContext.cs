using Bittly.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Bittly.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Url> Urls { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Url>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

           builder.Entity<Url>()
                    .HasIndex(u => u.ShortUrl)
                    .IsUnique();

            base.OnModelCreating(builder);
        }

    }

}

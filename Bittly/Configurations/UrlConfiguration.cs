using Bittly.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bittly.Configurations
{
    public class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.HasKey(u => u.UrlId);

            builder.Property(u => u.LongUrl)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.ShortUrl)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(u => u.ExiprationDate)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(u => u.ShortUrl)
                .IsUnique();
        }
    }
}

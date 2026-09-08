using Bittly.Models;
using Microsoft.EntityFrameworkCore;
namespace Bittly.Infrastructure
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Url?> GetByIdAsync(int urlId)
        {
            return await _context.Urls.FindAsync(urlId);
        }

        public async Task<Url?> GetByShortUrlAsync(string shortUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
        }

        public async Task<IEnumerable<Url>> GetUrlsByUserIdAsync(int userId)
        {
            return await _context.Urls
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Url url)
        {
            await _context.Urls.AddAsync(url);
        }

        public Task UpdateAsync(Url url)
        {
            _context.Urls.Update(url);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Url url)
        {
            _context.Urls.Remove(url);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}


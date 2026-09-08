using Bittly.Models;

namespace Bittly.Infrastructure
{
    public interface IUrlRepository
    {
        Task<Url?> GetByIdAsync(int urlId);
        Task<Url?> GetByShortUrlAsync(string shortUrl);
        Task<IEnumerable<Url>> GetUrlsByUserIdAsync(int userId);
        Task AddAsync(Url url);
        Task UpdateAsync(Url url);
        Task DeleteAsync(Url url);
        Task SaveChangesAsync(); 
    }
}

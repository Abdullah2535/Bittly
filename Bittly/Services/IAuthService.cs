using Bittly.Configurations;
using Bittly.Dtos;

namespace Bittly.Services
{
    public interface IAuthService
    {
        Task<Result<string>> RegisterAsync(RegisterDto dto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
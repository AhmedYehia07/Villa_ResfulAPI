using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto loginRequest);
        Task<T> RegisterAsync<T>(RegisterRequestDto registerRequest);
    }
}

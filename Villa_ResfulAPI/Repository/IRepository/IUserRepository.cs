using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;

namespace Villa_ResfulAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        public bool IsUniqueUser(string username);
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        public Task<UserDto> Register(RegisterRequestDto registerRequestDto);
    }
}

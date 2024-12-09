using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Repository
{
    public class UserRepository(ApplicationDbContext dbContext,IConfiguration configuration) : IUserRepository
    {
        public bool IsUniqueUser(string username)
        {
            var user = dbContext.Users.FirstOrDefault(u=>u.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = dbContext.Users.FirstOrDefault(u=>u.UserName.ToLower() == loginRequestDto.UserName.ToLower()
            && u.Password == loginRequestDto.Password);
            if(user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("ApiSettings:SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
        }

        public async Task<LocalUser> Register(RegisterRequestDto registerRequestDto)
        {
            var user = new LocalUser()
            {
                UserName = registerRequestDto.UserName,
                Password = registerRequestDto.Password,
                Name = registerRequestDto.Name,
                Role = registerRequestDto.Role
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}

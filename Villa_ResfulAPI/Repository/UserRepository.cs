using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Repository
{
    public class UserRepository(ApplicationDbContext dbContext,IConfiguration configuration,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper) : IUserRepository
    {
        public bool IsUniqueUser(string username)
        {
            var user = dbContext.applicationUsers.FirstOrDefault(u=>u.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = dbContext.applicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool IsValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if(user == null || IsValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            var role = await userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("ApiSettings:SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,role.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = mapper.Map<UserDto>(user),
                Role = role.FirstOrDefault()
            };
        }

        public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
        {
            var user = new ApplicationUser()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
                NormalizedEmail = registerRequestDto.UserName.ToUpper(),
                Name = registerRequestDto.Name,
            };
            try
            {
                var result = await userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    if(!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await roleManager.CreateAsync(new IdentityRole("admin"));
                    }
                    if(!roleManager.RoleExistsAsync("customer").GetAwaiter().GetResult())
                    {
                        await roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await userManager.AddToRoleAsync(user,registerRequestDto.Role);
                    var userToReturn = dbContext.applicationUsers.FirstOrDefault(u=>u.UserName == registerRequestDto.UserName);
                    return mapper.Map<UserDto>(userToReturn);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { }

            return null;
        }
    }
}

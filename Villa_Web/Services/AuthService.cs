using Microsoft.AspNetCore.Identity.Data;
using Villa_Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string? VillaURL;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
            VillaURL = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }
        ///api/Users/Login
        public Task<T> LoginAsync<T>(LoginRequestDto loginRequest)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Post,
                Data = loginRequest,
                URL = VillaURL + "api/Users/Login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterRequestDto registerRequest)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Post,
                Data = registerRequest,
                URL = VillaURL + "api/Users/Register"
            });
        }
    }
}

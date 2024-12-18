using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        protected APIResponse _response = new APIResponse();
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            var loginResponse = await userRepository.Login(login);
            if(loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.ErrorMessages.Add("Incorrect Username or Password");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            var unique = userRepository.IsUniqueUser(requestDto.UserName);
            if(unique == false)
            {
                _response.ErrorMessages.Add("Username already exists");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                return BadRequest(_response);
            }
            var User = await userRepository.Register(requestDto);
            if(User == null)
            {
                _response.ErrorMessages.Add("Error while register");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                return BadRequest(_response);
            }
            _response.Result = null;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}

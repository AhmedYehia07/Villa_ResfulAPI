using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        protected APIResponse _response = new APIResponse();
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            var loginRequest = await userRepository.Login(login);
            if(loginRequest.User == null || string.IsNullOrEmpty(loginRequest.Token))
            {
                _response.ErrorMessages.Add("Incorrect Username or Password");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                return BadRequest(_response);
            }
            _response.Result = loginRequest;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            var unique = userRepository.IsUniqueUser(requestDto.UserName);
            if(!unique)
            {
                _response.ErrorMessages.Add("Username already exists");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                return BadRequest(_response);
            }
            var localUser = await userRepository.Register(requestDto);
            if(localUser == null)
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

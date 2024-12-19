using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Villa_Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class AuthController(IAuthService AuthService) : Controller
    {
        public IActionResult Login()
        {
            var loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var response = await AuthService.LoginAsync<APIResponse>(loginRequest);
            if(response != null && response.IsSuccess == true)
            {
                var loginCreds = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginCreds.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principle = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principle);



                HttpContext.Session.SetString(SD.SessionToken, loginCreds.Token);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
            return View(loginRequest);
        }
        public IActionResult Register()
        {
            var rolesList = new List<SelectListItem> {
                new SelectListItem { Text = SD.Admin, Value = SD.Admin },
                new SelectListItem { Text = SD.Customer, Value = SD.Customer }
            };
            ViewBag.RoleList = rolesList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Role))
            {
                registerRequest.Role = SD.Customer;
            }
            var response = await AuthService.RegisterAsync<APIResponse>(registerRequest);
            if (response != null && response.IsSuccess == true)
            {
                return RedirectToAction("Login");
            }
            var rolesList = new List<SelectListItem> {
                new SelectListItem { Text = SD.Admin, Value = SD.Admin },
                new SelectListItem { Text = SD.Customer, Value = SD.Customer }
            };
            ViewBag.RoleList = rolesList;
            return View(registerRequest);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index","Home");
        }
        public IActionResult AccessDenied()
        {
            var loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }
    }
}

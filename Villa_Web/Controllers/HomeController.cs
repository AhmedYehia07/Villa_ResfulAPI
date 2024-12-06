using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class HomeController(IVillaService villaService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<VillaDto> list = new List<VillaDto>();
            var response = await villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}

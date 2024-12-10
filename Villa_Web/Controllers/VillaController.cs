using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
    public class VillaController(IVillaService villaService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<VillaDto> list = new List<VillaDto>();
            var response = await villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize]
        public IActionResult CreateVilla()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVilla(VillaCreateDto villaDto)
		{
			if(ModelState.IsValid)
			{
				var response = await villaService.CreateAsync<APIResponse>(villaDto, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess == true)
				{
					TempData["success"] = "Villa created sucessfully";
					return RedirectToAction("Index");
				}
			}
            TempData["error"] = "Failed to create Villa";
            return View(villaDto);
		}
        [Authorize]
        public async Task<IActionResult> UpdateVilla(int villaId)
		{
			var response = await villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess == true)
			{
				VillaDto villaDto = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(mapper.Map<VillaUpdateDto>(villaDto));
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVilla(VillaUpdateDto villaDto)
		{
			if (ModelState.IsValid)
			{
				var response = await villaService.UpdateAsync<APIResponse>(villaDto, HttpContext.Session.GetString(SD.SessionToken));
				if (response != null && response.IsSuccess == true)
				{
                    TempData["success"] = "Villa updated sucessfully";
                    return RedirectToAction("Index");
				}
			}
			return View(villaDto);
		}
        [Authorize]
        public async Task<IActionResult> DeleteVilla(int villaId)
		{
			var response = await villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess == true)
			{
				VillaDto villaDto = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
				return View(villaDto);
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVilla(VillaDto villaDto)
		{
			var response = await villaService.DeleteAsync<APIResponse>(villaDto.Id, HttpContext.Session.GetString(SD.SessionToken));
			if (response != null && response.IsSuccess == true)
			{
                TempData["success"] = "Villa deleted sucessfully";
                return RedirectToAction("Index");
			}
            TempData["error"] = "Failed to delete Villa";
            return View(villaDto);
		}
	}
}

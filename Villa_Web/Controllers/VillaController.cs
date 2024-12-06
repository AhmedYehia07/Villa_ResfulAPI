using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var response = await villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
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
				var response = await villaService.CreateAsync<APIResponse>(villaDto);
				if (response != null && response.IsSuccess == true)
				{
					return RedirectToAction("Index");
				}
			}
			return View(villaDto);
		}
		public async Task<IActionResult> UpdateVilla(int villaId)
		{
			var response = await villaService.GetAsync<APIResponse>(villaId);
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
				var response = await villaService.UpdateAsync<APIResponse>(villaDto);
				if (response != null && response.IsSuccess == true)
				{
					return RedirectToAction("Index");
				}
			}
			return View(villaDto);
		}
		public async Task<IActionResult> DeleteVilla(int villaId)
		{
			var response = await villaService.GetAsync<APIResponse>(villaId);
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
			var response = await villaService.DeleteAsync<APIResponse>(villaDto.Id);
			if (response != null && response.IsSuccess == true)
			{
				return RedirectToAction("Index");
			}
			return View(villaDto);
		}
	}
}

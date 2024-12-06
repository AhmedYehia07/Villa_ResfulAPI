using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
	public class VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper) : Controller
	{
		public async Task<IActionResult> Index()
		{
			List<VillaNumberDto> list = new List<VillaNumberDto>();
			var response = await villaNumberService.GetAllAsync<APIResponse>();
			if (response != null && response.IsSuccess == true)
			{
				list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
			}
			return View(list);
		}
		public IActionResult CreateVillaNumber()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDto createDto)
		{
			var response = await villaNumberService.CreateAsync<APIResponse>(createDto);
			if (response != null && response.IsSuccess == true)
			{
				return RedirectToAction("Index");
			}
			return View(createDto);
		}
		public async Task<IActionResult> UpdateVillaNumber(int villaNo)
		{
			var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
			if (response != null && response.IsSuccess == true)
			{
				var villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				return View(mapper.Map<VillaNumberUpdateDto>(villaNumberDto));
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDto UpdateDto)
		{
			if (ModelState.IsValid)
			{
				var response = await villaNumberService.UpdateAsync<APIResponse>(UpdateDto);
				if (response != null && response.IsSuccess == true)
				{
					return RedirectToAction("Index");
				}
			}
			return View(UpdateDto);
		}
		public async Task<IActionResult> DeleteVillaNumber(int villaNo)
		{
			var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
			if (response != null && response.IsSuccess == true)
			{
				var villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				return View(villaNumberDto);
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVillaNumber(VillaNumberDto VillaNumberDto)
		{
			var response = await villaNumberService.DeleteAsync<APIResponse>(VillaNumberDto.VillaNo);
			if (response != null && response.IsSuccess == true)
			{
				return RedirectToAction("Index");
			}
			return View(VillaNumberDto);
		}
	}
}
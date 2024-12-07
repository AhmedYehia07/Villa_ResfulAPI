using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Models.VM;
using Villa_Web.Services.IServices;

namespace Villa_Web.Controllers
{
	public class VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService) : Controller
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
		public async Task<IActionResult> CreateVillaNumber()
		{
			var villaNumberVm = new VillaNumberCreateVm();
			var response = await villaService.GetAllAsync<APIResponse>();
			if(response != null && response.IsSuccess == true)
			{
				villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)).Select(i =>
				new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				});
			}
			return View(villaNumberVm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVm createVm)
		{
			if(ModelState.IsValid)
			{
				var response = await villaNumberService.CreateAsync<APIResponse>(createVm.VillaNumber);
				if (response != null && response.IsSuccess == true)
				{
                    TempData["success"] = "Villa Number created sucessfully";
                    return RedirectToAction("Index");
				}
			}
            TempData["error"] = "Failed to create Villa number";
            return View(createVm);
		}
		public async Task<IActionResult> UpdateVillaNumber(int villaNo)
		{
			var villaNumberVm = new VillaNumberUpdateVm();
			var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
			if (response != null && response.IsSuccess == true)
			{
				var villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				villaNumberVm.VillaNumber = mapper.Map<VillaNumberUpdateDto>(villaNumberDto);
				response = await villaService.GetAllAsync<APIResponse>();
				if(response != null && response.IsSuccess == true)
				{
					villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)).Select(i =>
					new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
					return View(villaNumberVm);
				}
			}
			return NotFound();
			
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVm UpdateDto)
		{
			if (ModelState.IsValid)
			{
				var response = await villaNumberService.UpdateAsync<APIResponse>(UpdateDto.VillaNumber);
				if (response != null && response.IsSuccess == true)
				{
                    TempData["success"] = "Villa Number updated sucessfully";
                    return RedirectToAction("Index");
				}
			}
			return View(UpdateDto);
		}
		public async Task<IActionResult> DeleteVillaNumber(int villaNo)
		{
			var villaNumberVm = new VillaNumberUpdateVm();
			var response = await villaNumberService.GetAsync<APIResponse>(villaNo);
			if (response != null && response.IsSuccess == true)
			{
				var villaNumberDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
				villaNumberVm.VillaNumber = mapper.Map<VillaNumberUpdateDto>(villaNumberDto);
				response = await villaService.GetAllAsync<APIResponse>();
				if (response != null && response.IsSuccess == true)
				{
					villaNumberVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)).Select(i =>
					new SelectListItem
					{
						Text = i.Name,
						Value = i.Id.ToString()
					});
					return View(villaNumberVm);
				}
            }
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteVillaNumber(VillaNumberUpdateVm VillaNumberDto)
		{
			var response = await villaNumberService.DeleteAsync<APIResponse>(VillaNumberDto.VillaNumber.VillaNo);
			if (response != null && response.IsSuccess == true)
			{
                TempData["success"] = "Villa Number deleted sucessfully";
                return RedirectToAction("Index");
			}
			TempData["error"] = "Failed to delete Villa number";
			return View(VillaNumberDto);
		}
	}
}
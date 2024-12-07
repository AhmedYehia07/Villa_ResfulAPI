using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_Web.Models.DTO;

namespace Villa_Web.Models.VM
{
	public class VillaNumberCreateVm
	{
        public VillaNumberCreateDto VillaNumber {  get; set; }
        public VillaNumberCreateVm()
        {
            VillaNumber = new VillaNumberCreateDto();
        }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_Web.Models.DTO;

namespace Villa_Web.Models.VM
{
	public class VillaNumberUpdateVm
	{
        public VillaNumberUpdateDto VillaNumber {  get; set; }
        public VillaNumberUpdateVm()
        {
            VillaNumber = new VillaNumberUpdateDto();
        }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}

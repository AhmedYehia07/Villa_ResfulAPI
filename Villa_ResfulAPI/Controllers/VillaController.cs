using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Logging;
using Villa_ResfulAPI.Models.DTO;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController(ILogging logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<VillaDto>> GetVillas()
        {
            logger.Log("Getting all villas","");
            return Ok(VillaStore.listVilla);
        }
        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type = typeof(VillaDto))]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id == 0)
            {
                logger.Log("Get villa error with id " + id,"error");
                return BadRequest();
            }
            var villa = VillaStore.listVilla.FirstOrDefault(i => i.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villa)
        {
            if(VillaStore.listVilla.FirstOrDefault(u=> u.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customError", "Name Exists");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }
            else if(villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id = VillaStore.listVilla.OrderByDescending(u=> u.Id).First().Id+1;
            VillaStore.listVilla.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id },villa); //to return the location of the created villa we call GetVilla method and pass the id
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.listVilla.Remove(villa);
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id,[FromBody]VillaDto villa)
        {
            if(id==0 || id!=villa.Id)
            {
                return BadRequest();
            }
            var newvilla = VillaStore.listVilla.FirstOrDefault(u=>u.Id == id);
            newvilla.Name = villa.Name;
            newvilla.Occupancy = villa.Occupancy;
            newvilla.Sqft = villa.Sqft;
            return NoContent();
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDto> PatchVilla)
        {
            if(id == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.listVilla.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            PatchVilla.ApplyTo(villa,ModelState); //Apply new changes to the old villa and add state to 'ModelState'
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}

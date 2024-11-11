using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController(ILogger<VillaController> logger,IVillaRepository dbVilla,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<VillaDto>>> GetVillas()
        {
            var villaList = await dbVilla.GetAllAsync();
            return Ok(mapper.Map<List<VillaDto>>(villaList));
        }
        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type = typeof(VillaDto))]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if(id == 0)
            {
                logger.LogError("Get villa error with id " + id);
                return BadRequest();
            }
            var villa = await dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<VillaDto>(villa));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody]VillaCreateDto villaDto)
        {
            if(await dbVilla.GetAsync(u=> u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customError", "Name Exists");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            Villa newVilla = mapper.Map<Villa>(villaDto);
            await dbVilla.CreateAsync(newVilla);

            return CreatedAtRoute("GetVilla", new { id = newVilla.Id },newVilla); //to return the location of the created villa we call GetVilla method and pass the id
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = await dbVilla.GetAsync(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            await dbVilla.RemoveAsync(villa);
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id,[FromBody]VillaUpdateDto villaDto)
        {
            if(id==0 || id!=villaDto.Id)
            {
                return BadRequest();
            }
            Villa villaupdate = mapper.Map<Villa>(villaDto);
            await dbVilla.UpdateAsync(villaupdate);
            return NoContent();
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int? id,JsonPatchDocument<VillaUpdateDto> PatchVilla)
        {
            if(id == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await dbVilla.GetAsync(u => u.Id == id,false);
            if (villa == null)
            {
                return NotFound();
            }
            VillaUpdateDto villaDto = mapper.Map<VillaUpdateDto>(villa);
            PatchVilla.ApplyTo(villaDto,ModelState); //Apply new changes to the old villa and add state to 'ModelState'
            Villa villaPart = mapper.Map<Villa>(villaDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await dbVilla.UpdateAsync(villaPart);
            return NoContent();
        }
    }
}

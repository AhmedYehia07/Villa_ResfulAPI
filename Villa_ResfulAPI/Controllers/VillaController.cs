using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController(ILogger<VillaController> logger,ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<VillaDto>> GetVillas()
        {
            var villaList = dbContext.villas.ToList();
            return Ok(villaList);
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
                logger.LogError("Get villa error with id " + id);
                return BadRequest();
            }
            var villa = dbContext.villas.FirstOrDefault(u => u.Id == id);
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
        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDto)
        {
            if(dbContext.villas.FirstOrDefault(u=> u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("customError", "Name Exists");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            else if(villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa newVilla = new()
            {
                Name = villaDto.Name,
                Details = villaDto.Details,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft,
                Occupancy = villaDto.Occupancy,
                Amenity = villaDto.Amenity,
                ImageUrl = villaDto.ImageUrl
            };
            dbContext.villas.Add(newVilla);
            dbContext.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDto.Id },villaDto); //to return the location of the created villa we call GetVilla method and pass the id
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
            var villa = dbContext.villas.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            dbContext.villas.Remove(villa);
            dbContext.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id,[FromBody]VillaDto villaDto)
        {
            if(id==0 || id!=villaDto.Id)
            {
                return BadRequest();
            }
            Villa villaupdate = new()
            {
                Id = id,
                Name = villaDto.Name,
                Details = villaDto.Details,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft,
                Occupancy = villaDto.Occupancy,
                Amenity = villaDto.Amenity,
                ImageUrl = villaDto.ImageUrl
            };
            dbContext.villas.Update(villaupdate);
            dbContext.SaveChanges();
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
            var villa = dbContext.villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            VillaDto villaDto = new()
            {
                Id = id,
                Name = villa.Name,
                Details = villa.Details,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy,
                Amenity = villa.Amenity,
                ImageUrl = villa.ImageUrl
            };
            if(villa == null)
            {
                return NotFound();
            }
            PatchVilla.ApplyTo(villaDto,ModelState); //Apply new changes to the old villa and add state to 'ModelState'
            Villa villaPart = new()
            {
                Id = id,
                Name = villaDto.Name,
                Details = villaDto.Details,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft,
                Occupancy = villaDto.Occupancy,
                Amenity = villaDto.Amenity,
                ImageUrl = villaDto.ImageUrl
            };
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dbContext.villas.Update(villaPart);
            dbContext.SaveChanges();
            return NoContent();
        }
    }
}

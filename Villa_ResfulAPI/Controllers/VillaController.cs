using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper mapper;
        private readonly IVillaRepository dbVilla;
        private readonly ILogger<VillaController> logger;
        public VillaController(ILogger<VillaController> _logger, IVillaRepository _dbVilla, IMapper _mapper)
        {
            mapper = _mapper;
            dbVilla = _dbVilla;
            logger = _logger;
            this._response = new();
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var villaList = await dbVilla.GetAllAsync();
                _response.Result = mapper.Map<List<VillaDto>>(villaList);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpGet("{id:int}", Name ="GetVilla")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type = typeof(VillaDto))]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    logger.LogError("Get villa error with id " + id);
                    return BadRequest();
                }
                var villa = await dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                _response.Result = mapper.Map<VillaDto>(villa);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDto villaDto)
        {
            try
            {
                if (await dbVilla.GetAsync(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
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

                _response.Result = mapper.Map<VillaDto>(newVilla);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetVilla", new { id = newVilla.Id }, _response); //to return the location of the created villa we call GetVilla method and pass the id

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Custom")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                await dbVilla.RemoveAsync(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id,[FromBody]VillaUpdateDto UpdateDto)
        {
            try
            {
                if (id == 0 || id != UpdateDto.Id)
                {
                    return BadRequest();
                }
                var villaDto = mapper.Map<VillaDto>(UpdateDto);
                Villa villaupdate = mapper.Map<Villa>(villaDto);
                await dbVilla.UpdateAsync(villaupdate);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int? id,JsonPatchDocument<VillaUpdateDto> PatchVilla)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return BadRequest();
                }
                var villa = await dbVilla.GetAsync(u => u.Id == id, false);
                if (villa == null)
                {
                    return NotFound();
                }
                VillaUpdateDto villaDto = mapper.Map<VillaUpdateDto>(villa);
                PatchVilla.ApplyTo(villaDto, ModelState); //Apply new changes to the old villa and add state to 'ModelState'
                Villa villaPart = mapper.Map<Villa>(villaDto);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await dbVilla.UpdateAsync(villaPart);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}

using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Models.DTO;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Controllers.V2
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbvillaNumber;
        private readonly IVillaRepository _dbvilla;
        private readonly ILogger _logger;
        public VillaNumberController(ILogger<VillaNumberController> logger,IVillaNumberRepository dbvillaNumber, IVillaRepository dbvilla,IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _dbvillaNumber = dbvillaNumber;
            _dbvilla = dbvilla;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<APIResponse>> GetAllVillaNumbers()
        {
            try
            {
                var VillaNumbers = await _dbvillaNumber.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDto>>(VillaNumbers);
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpGet("{villaNo:int}",Name ="GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if(villaNo >= 1000 || villaNo <= 100)
                {
                    _response.IsSuccess=false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "VillaNumber range from 100 to 1000" };
                    return BadRequest(_response);
                }
                var VillaNumber = await _dbvillaNumber.GetAsync(u=>u.VillaNo==villaNo, includeProperties: "Villa");
                if(VillaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "VillaNumber not found" };
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDto>(VillaNumber);
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber(VillaNumberCreateDto createDto)
        {
            try
            {
                if (createDto.VillaNo >= 1000 || createDto.VillaNo <= 100)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Number range from 100 to 1000" };
                    return BadRequest(_response);
                }
                if(await _dbvilla.GetAsync(u=>u.Id == createDto.VillaID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "VillaID not found" };
                    return NotFound(_response);
                }
                var villanumber = _mapper.Map<VillaNumber>(createDto);
                villanumber.CreatedDate = DateTime.Now;
                await _dbvillaNumber.CreateAsync(villanumber);
                _response.IsSuccess = true;
                _response.StatusCode= HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDto>(villanumber);
                return CreatedAtRoute("GetVillaNumber", new { villaNo = villanumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }
        [HttpDelete("{VillaNo:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int VillaNo)
        {
            try
            {
                if (VillaNo >= 1000 || VillaNo <= 100)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Number range from 100 to 1000" };
                    return BadRequest(_response);
                }
                var VillaNumber = await _dbvillaNumber.GetAsync(u => u.VillaNo == VillaNo);
                if (VillaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "Villa Number not found" };
                    return NotFound(_response);
                }
                await _dbvillaNumber.RemoveAsync(VillaNumber);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
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
        [HttpPut("{VillaNo:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int VillaNo,VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (VillaNo != updateDto.VillaNo)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "Error validating Villa Number" };
                    return BadRequest(_response);
                }
                if (await _dbvilla.GetAsync(u => u.Id == updateDto.VillaID) == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "VillaID not found" };
                    return NotFound(_response);
                }
                var villaNumber = await _dbvillaNumber.GetAsync(u=>u.VillaNo == VillaNo,false);
                if(villaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string>() { "Villa Number not found" };
                    return NotFound(_response);
                }
                villaNumber = _mapper.Map<VillaNumber>(updateDto); 
                villaNumber.UpdatedDate = DateTime.Now;
                await _dbvillaNumber.UpdateAsync(villaNumber);
                _response.Result = updateDto;
                _response.IsSuccess = true;
                _response.StatusCode=HttpStatusCode.OK;
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
using Villa_Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using static System.Net.WebRequestMethods;

namespace Villa_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private string? VillaURL;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
            VillaURL = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDto entity)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Post,
                Data = entity,
                URL = VillaURL + "/api/VillaNumber"
			});
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Delete,
                URL = VillaURL + "/api/VillaNumber/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Get,
                URL = VillaURL + "/api/VillaNumber"
			});
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Get,
                URL = VillaURL + "/api/VillaNumber/" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDto entity)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Put,
                Data = entity,
                URL = VillaURL + "/api/VillaNumber/" + entity.VillaNo
            });
        }
    }
}

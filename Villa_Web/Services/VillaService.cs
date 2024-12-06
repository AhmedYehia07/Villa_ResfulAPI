using Villa_Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;
using static System.Net.WebRequestMethods;

namespace Villa_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string? VillaURL;
        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
            VillaURL = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }
        public Task<T> CreateAsync<T>(VillaCreateDto entity)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Post,
                Data = entity,
                URL = VillaURL + "/api/Villa"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Delete,
                URL = VillaURL + "/api/Villa/"+id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Get,
                URL = VillaURL + "/api/Villa"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Get,
                URL = VillaURL + "/api/Villa/" + id
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDto entity)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.Put,
                Data = entity,
                URL = VillaURL + "/api/Villa/" + entity.Id
            });
        }
    }
}

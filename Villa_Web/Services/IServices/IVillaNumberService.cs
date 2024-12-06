using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDto entity);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDto entity);
        Task<T> DeleteAsync<T>(int id);
    }
}

using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDto entity);
        Task<T> UpdateAsync<T>(VillaUpdateDto entity);
        Task<T> DeleteAsync<T>(int id);
    }
}

using Villa_ResfulAPI.Models;

namespace Villa_ResfulAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task UpdateAsync(VillaNumber entity);
    }
}

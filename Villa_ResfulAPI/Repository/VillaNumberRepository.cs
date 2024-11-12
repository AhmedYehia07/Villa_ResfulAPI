using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext dbContext;
        public VillaNumberRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            dbContext.VillaNumbers.Update(entity);
            await SaveAsync();
        }
    }
}

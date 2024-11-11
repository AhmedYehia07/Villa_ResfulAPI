using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_ResfulAPI.Data;
using Villa_ResfulAPI.Models;
using Villa_ResfulAPI.Repository.IRepository;

namespace Villa_ResfulAPI.Repository
{
    public class VillaRepository : Repository<Villa>,IVillaRepository
    {
        private readonly ApplicationDbContext dbContext;
        public VillaRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            dbContext.Update(entity);
            await SaveAsync();
        }

        public Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
    }
}

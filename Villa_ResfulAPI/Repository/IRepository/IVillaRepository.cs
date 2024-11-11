using System.Linq.Expressions;
using Villa_ResfulAPI.Models;

namespace Villa_ResfulAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task UpdateAsync(Villa entity);
    }
}

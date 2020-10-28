using RTL.CastAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data
{
    public interface ICastRepository
    {
        Task AddAsync(Show show);
        Task<int?> GetMaxExternalIdAsync();
        Task<IEnumerable<Show>> GetAllAsync(int page, int pageSize);
    }
}

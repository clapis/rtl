using RTL.CastAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data
{
    public interface IShowsRepository
    {
        Task AddAsync(Show show);
        Task<int> GetMaxExternalIdAsync();
        Task<Show> FindByExternalIdAsync(int externalId);
        Task<List<Show>> GetPageAsync(int page, int pageSize);

    }
}

using Microsoft.EntityFrameworkCore;
using RTL.CastAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data.EFCore
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly CastDBContext _context;

        public ShowsRepository(CastDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Show show)
        {
            _context.Shows.Attach(show);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetMaxExternalIdAsync()
        {
            return await _context.Shows.Select(x => x.ExternalId)
                                        .DefaultIfEmpty(default)
                                        .MaxAsync();
        }

        public Task<Show> FindByExternalIdAsync(int externalId)
        {
            return _context.Shows.FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task<List<Show>> GetPageAsync(int page, int pageSize)
        {
            var results = await _context.Shows
                            .AsNoTracking()
                            .OrderBy(x => x.Id)
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .Include(x => x.Cast)
                            .ThenInclude(x => x.Person)
                            .ToListAsync();
            return results;
        }
    }
}

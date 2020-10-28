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
            var any = await _context.Shows.AnyAsync();

            if (!any) return 0;

            return await _context.Shows.MaxAsync(s => s.Id);
        }

        public Task<Show> FindByExternalIdAsync(int externalId)
        {
            return _context.Shows.FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task<List<Show>> GetPageAsync(int page, int pageSize)
        {
            var results = await _context.Shows
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

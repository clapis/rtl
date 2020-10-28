using Microsoft.EntityFrameworkCore;
using RTL.CastAPI.Model;
using System;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data.EFCore
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly CastDBContext _context;

        public PeopleRepository(CastDBContext context)
        {
            _context = context;
        }

        public Task<Person> FindByExternalIdAsync(int externalId)
        {
            return _context.People.FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }
    }
}

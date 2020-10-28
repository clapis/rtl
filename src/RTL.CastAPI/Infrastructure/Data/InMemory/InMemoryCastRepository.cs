using RTL.CastAPI.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data.InMemory
{
    public class InMemoryCastRepository : ICastRepository
    {
        private static readonly ConcurrentDictionary<int, Show> IdentityMap = new ConcurrentDictionary<int, Show>();

        public Task<int?> GetMaxExternalIdAsync()
        {
            var max = IdentityMap.Any() ? IdentityMap.Keys.Max() : default(int?);

            return Task.FromResult(max);
        }

        public Task AddAsync(Show show)
        {
            IdentityMap.TryAdd(show.Id, show);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Show>> GetAllAsync(int page, int pageSize)
        {
            var results = IdentityMap
                .OrderBy(k => k.Key)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(s => DeepCopy(s.Value)); // return a copy, so that entries cannot be modified outside repo

            return Task.FromResult(results);
        }

        private Show DeepCopy(Show show)
        {
            return new Show
            {
                Id = show.Id,
                Name = show.Name,
                Cast = show.Cast.Select(DeepCopy)
                        .OrderByDescending(c => c.Person.Birthday)
                        .ToList()
            };
        }

        private CastMember DeepCopy(CastMember member)
        {
            return new CastMember
            {
                Person = new Person
                {
                    Id = member.Person.Id,
                    Name = member.Person.Name,
                    Birthday = member.Person.Birthday
                }
            };
        }
    }
}

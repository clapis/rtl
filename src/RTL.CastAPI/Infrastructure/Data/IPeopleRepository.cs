using RTL.CastAPI.Model;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.Data
{
    public interface IPeopleRepository
    {
        Task<Person> FindByExternalIdAsync(int externalId);
    }
}

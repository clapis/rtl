using RTL.CastAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.TvMaze
{
    public interface ITvMazeHttpClient
    {
        Task<IEnumerable<Show>> GetShowIndexPage(int page = 0);
        Task<IEnumerable<CastMember>> GetShowCastAsync(int showId);
    }
}
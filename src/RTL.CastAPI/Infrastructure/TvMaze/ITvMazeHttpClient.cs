using RTL.CastAPI.Infrastructure.TvMaze.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.TvMaze
{
    public interface ITvMazeHttpClient
    {
        Task<List<Show>> GetShowIndexPage(int page = 0);
        Task<List<CastMember>> GetShowCastAsync(int showId);
    }
}
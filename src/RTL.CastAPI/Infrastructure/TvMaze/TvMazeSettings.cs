using System;

namespace RTL.CastAPI.Infrastructure.TvMaze
{
    public class TvMazeSettings
    {
        public Uri BaseUrl { get; set; }
        public TimeSpan RateLimit { get; set; }
    }
}

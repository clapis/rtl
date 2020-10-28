using System.Collections.Generic;

namespace RTL.CastAPI.Infrastructure.TvMaze.Contract
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastMember> Cast { get; set; }
    }
}

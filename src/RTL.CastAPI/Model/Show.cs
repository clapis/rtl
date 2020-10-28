using System.Collections.Generic;

namespace RTL.CastAPI.Model
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastMember> Cast { get; set; }
    }
}

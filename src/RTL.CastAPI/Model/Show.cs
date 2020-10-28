using System.Collections.Generic;
namespace RTL.CastAPI.Model
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExternalId { get; set; }
        public List<CastMember> Cast { get; set; }

        public Show()
        {
            Cast = new List<CastMember>();
        }

        public Show(string name, int externalId) : this()
        {
            Name = name;
            ExternalId = externalId;
        }

        public void AddCastMember(Person person)
        {
            Cast.Add(new CastMember(this, person));
        }
    }
}

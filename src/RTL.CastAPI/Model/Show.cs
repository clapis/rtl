using System.Collections.Generic;
using System.Linq;

namespace RTL.CastAPI.Model
{
    public class Show
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int ExternalId { get; private set; }

        private List<CastMember> _cast = new List<CastMember>();
        public IReadOnlyList<CastMember> Cast => _cast.ToList();

        public Show(string name, int externalId)
        {
            Name = name;
            ExternalId = externalId;
        }

        public void AddCastMember(Person person)
        {
            // If person is already in cast, nothing to be done
            // TODO: override person equality so we abstract ids away
            if (person.Id != 0 && _cast.Any(m => m.PersonId == person.Id))
                return;

            _cast.Add(new CastMember(this, person));
        }
    }
}

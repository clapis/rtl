namespace RTL.CastAPI.Model
{
    public class CastMember
    {

        public int ShowId { get; private set; }
        public Show Show { get; private set; }

        public int PersonId { get; private set; }
        public Person Person { get; private set; }

        private CastMember() { }

        public CastMember(Show show, Person person) : this()
        {
            Show = show;
            ShowId = show.Id;
            Person = person;
            PersonId = person.Id;
        }

    }
}

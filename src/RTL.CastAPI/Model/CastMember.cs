namespace RTL.CastAPI.Model
{
    public class CastMember
    {

        public int ShowId { get; set; }
        public Show Show { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public CastMember()
        {

        }

        public CastMember(Show show, Person person) : this()
        {
            Show = show;
            ShowId = show.Id;
            Person = person;
            PersonId = person.Id;
        }

    }
}

using System;

namespace RTL.CastAPI.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int ExternalId { get; set; }

        public Person()
        {

        }

        public Person(string name, int externalid, DateTime? birthday) : this()
        {
            Name = name;
            ExternalId = externalid;
            Birthday = birthday;
        }

    }
}

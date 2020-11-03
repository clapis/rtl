using System;

namespace RTL.CastAPI.Model
{
    public class Person
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int ExternalId { get; private set; }
        public DateTime? Birthday { get; private set; }

        public Person(string name, int externalId, DateTime? birthday) 
        {
            Name = name;
            Birthday = birthday;
            ExternalId = externalId;
        }

    }
}

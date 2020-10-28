using System;
using System.Collections.Generic;

namespace RTL.CastAPI.Application.Queries.GetAllShowsCast
{
    public class GetAllShowsQueryResult
    {
        public List<Show> Shows { get; set; }

        public class Show
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public List<Person> Cast { get; set; }
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Birthday { get; set; }
        }

    }
}

using AutoMapper;
using RTL.CastAPI.Extensions;
using RTL.CastAPI.Model;
using System.Linq;

namespace RTL.CastAPI.Application.Queries.GetAllShowsCast
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Show, GetAllShowsQueryResult.Show>()
                // Order cast by birthday descending. Not ideal, but fow now will do.
                .AfterMap((src, dst) => dst.Cast = dst.Cast.OrderByDescending(x => x.Birthday).ToList());

            CreateMap<CastMember, GetAllShowsQueryResult.Person>()
                .IncludeMembers(x => x.Person);

            CreateMap<Person, GetAllShowsQueryResult.Person>()
                .ForMember(x => x.Birthday, opts => opts.MapFrom(src => src.Birthday.ToDateFormat()));
        }
    }
}

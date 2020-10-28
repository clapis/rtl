using AutoMapper;
using RTL.CastAPI.Extensions;
using RTL.CastAPI.Model;

namespace RTL.CastAPI.Application.Queries.GetAllShowsCast
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Show, GetAllShowsQueryResult.Show>()
                .ForMember(x => x.Cast, opts => opts.MapFrom(s => s.Cast));

            CreateMap<CastMember, GetAllShowsQueryResult.Person>()
                .IncludeMembers(x => x.Person);

            CreateMap<Person, GetAllShowsQueryResult.Person>()
                .ForMember(x => x.Birthday, opts => opts.MapFrom(src => src.Birthday.ToDateFormat()));
        }
    }
}

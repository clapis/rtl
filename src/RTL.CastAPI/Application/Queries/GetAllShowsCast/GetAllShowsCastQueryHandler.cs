using AutoMapper;
using MediatR;
using RTL.CastAPI.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RTL.CastAPI.Application.Queries.GetAllShowsCast
{
    public class GetAllShowsCastQueryHandler : IRequestHandler<GetAllShowsQuery, GetAllShowsQueryResult>
    {
        private readonly IShowsRepository _repository;
        private readonly IMapper _mapper;

        public GetAllShowsCastQueryHandler(
            IShowsRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllShowsQueryResult> Handle(GetAllShowsQuery request, CancellationToken cancellationToken)
        {
            var shows = await _repository.GetPageAsync(request.Page, request.PageSize);

            // Order cast by birthday descending. Not ideal, but fow now will do.
            shows.ForEach(s => s.Cast = s.Cast.OrderByDescending(p => p.Person?.Birthday).ToList());

            var dto = _mapper.Map<List<GetAllShowsQueryResult.Show>>(shows);

            return new GetAllShowsQueryResult { Shows = dto };
        }

    }
}

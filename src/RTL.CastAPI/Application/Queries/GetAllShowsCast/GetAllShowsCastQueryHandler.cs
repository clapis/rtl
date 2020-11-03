using AutoMapper;
using MediatR;
using RTL.CastAPI.Infrastructure.Data;
using System.Collections.Generic;
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

            var dto = _mapper.Map<List<GetAllShowsQueryResult.Show>>(shows);

            return new GetAllShowsQueryResult { Shows = dto };
        }

    }
}

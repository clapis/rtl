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
        private readonly ICastRepository _repository;
        private readonly IMapper _mapper;

        public GetAllShowsCastQueryHandler(
            ICastRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetAllShowsQueryResult> Handle(GetAllShowsQuery request, CancellationToken cancellationToken)
        {
            var shows = await _repository.GetAllAsync(request.Page, request.PageSize);

            var dto = _mapper.Map<List<GetAllShowsQueryResult.Show>>(shows);

            return new GetAllShowsQueryResult { Shows = dto };
        }

    }
}

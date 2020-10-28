using MediatR;

namespace RTL.CastAPI.Application.Queries.GetAllShowsCast
{
    public class GetAllShowsQuery : IRequest<GetAllShowsQueryResult>
    {
        public int Page { get; }

        public int PageSize { get; } = 20;

        public GetAllShowsQuery(int page)
        {
            Page = page;
        }
    }
}

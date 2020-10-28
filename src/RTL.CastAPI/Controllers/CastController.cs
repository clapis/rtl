using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RTL.CastAPI.Application.Queries.GetAllShowsCast;

namespace RTL.CastAPI.Controllers
{
    [ApiController]
    [Route("cast")]
    public class CastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /cast?page=0
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<GetAllShowsQueryResult.Show>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int page = 0)
        {
            if (page < 0)
                return BadRequest($"Page cannot be a negative number: '{page}'");

            var result = await _mediator.Send(new GetAllShowsQuery(page));

            return Ok(result.Shows);
        }
    }
}

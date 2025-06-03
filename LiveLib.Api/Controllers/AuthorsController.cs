using LiveLib.Api.Common;
using LiveLib.Application.Features.Authors.CreateAuthor;
using LiveLib.Application.Features.Authors.DeleteAuthor;
using LiveLib.Application.Features.Authors.GetAuthorById;
using LiveLib.Application.Features.Authors.GetAuthors;
using LiveLib.Application.Features.Authors.UpdateAuthor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLib.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthorsController : ControllerApiBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors(CancellationToken ct)
        {
            var authors = await _mediator.Send(new GetAuthorsQuery(), ct);
            return Ok(authors);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailGenre([FromRoute] Guid id, CancellationToken ct)
        {
            var get = await _mediator.Send(new GetAuthorByIdQuery(id), ct);
            return ToActionResult(get);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddGenre([FromBody] CreateAuthorCommand reqwest, CancellationToken ct)
        {
            var create = await _mediator.Send(reqwest, ct);
            return ToActionResult(create);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAuthorDto updatedAuthor, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateAuthorCommand(id, updatedAuthor), ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id), ct);
            return ToActionResult(result);
        }
    }
}

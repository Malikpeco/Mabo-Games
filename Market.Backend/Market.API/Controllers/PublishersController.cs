using Market.Application.Modules.Publishers.Commands.Create;
using Market.Application.Modules.Publishers.Commands.Delete;
using Market.Application.Modules.Publishers.Commands.Update;
using Market.Application.Modules.Publishers.Queries.GetById;
using Market.Application.Modules.Publishers.Queries.List;
using System.Runtime.CompilerServices;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/publishers")]
    public sealed class PublishersController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<PageResult<ListPublishersQueryDto>> List([FromQuery] ListPublishersQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpGet("{id:int}")]
        public async Task<GetPublisherByIdQueryDto> GetById([FromRoute] int id, CancellationToken ct)
        {
            return await sender.Send(new GetPublisherByIdQuery { Id = id }, ct);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePublisherCommand command, CancellationToken ct)
        {
            int pubId = await sender.Send(command, ct);
            return CreatedAtAction("GetById", new { id = pubId }, pubId);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute]int id, UpdatePublisherCommand command, CancellationToken ct)
        {
            command.Id= id;
            await sender.Send(command, ct);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            return Ok(await sender.Send(new DeletePublisherCommand { Id = id }, ct));
        }
    }
}

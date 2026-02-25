using Market.Application.Modules.Games.Commands.Create;
using Market.Application.Modules.Games.Commands.Delete;
using Market.Application.Modules.Games.Commands.Update;
using Market.Application.Modules.Games.Dto;
using Market.Application.Modules.Games.Queries.GetGameDetails;
using Market.Application.Modules.Games.Queries.GetStorefrontGames;
using Market.Application.Modules.Games.Queries.ListGamesAutocompleteQuery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/games")]
    public sealed class GamesController(ISender sender) : ControllerBase
    {

        //get all games
        [HttpGet("storefront")]
        [AllowAnonymous]
        public async Task<PageResult<StorefrontGameDto>> GetStorefront([FromQuery] GetStorefrontGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }


            
        //get game by id
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<GameDetailsDto> GetGameDetails(int id, CancellationToken ct)
        {
            return await sender.Send(new GetGameDetailsQuery { Id = id }, ct);
        }



        //add new game
        [HttpPost]
        public async Task<ActionResult> Create(CreateGameCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Created();
        }


        //update existing game
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(UpdateGameCommand command, int id, CancellationToken ct)
        {
            command.Id = id;
            await sender.Send(command, ct);
            return NoContent();
        }



        //delete game
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteGameCommand {Id = id}, ct);
            return NoContent();
        }


        //autocomplete search
        [AllowAnonymous]
        [HttpGet("autocomplete")]
        public async Task<List<ListGamesAutocompleteQueryDto>> Autocomplete([FromQuery] string term, [FromQuery] int? genreId, CancellationToken ct)
        {
            return await sender.Send(new ListGamesAutocompleteQuery { Term = term, GenreId = genreId }, ct);
        } 



    }

    
}

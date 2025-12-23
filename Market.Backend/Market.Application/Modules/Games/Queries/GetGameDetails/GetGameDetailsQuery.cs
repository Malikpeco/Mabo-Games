using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.GetGameDetails
{
    public sealed class GetGameDetailsQuery : IRequest<GameDetailsDto>
    {
        public int Id { get; init; }
    }
}

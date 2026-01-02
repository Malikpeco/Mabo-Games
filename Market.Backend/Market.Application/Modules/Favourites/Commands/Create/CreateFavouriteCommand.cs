using Market.Application.Modules.Games.Dto;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Favourites.Commands.Create
{
    public sealed class CreateFavouriteCommand : IRequest<Unit>
    {
        public int GameId { get; set; }
    }
}

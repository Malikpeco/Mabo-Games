using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Favourites.Commands.Delete
{
    public sealed class DeleteFavouriteCommand : IRequest<Unit>
    {
        public int GameId { get; set; }
    }
}
